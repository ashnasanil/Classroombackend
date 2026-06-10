pipeline {
 
    agent any
 
    environment {
        IMAGE = "google-classroom-api-jenkins:${BUILD_NUMBER}"
        NETWORK = "google-classroom-jenkins-net"
        MYSQL_CONT = "google-classroom-mysql-jenkins"
        API_CONT = "google-classroom-api-jenkins"
 
        MYSQL_PWD = "root"
        MYSQL_DB = "GoogleClassroomDb"
    }
 
    stages {
 
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
 
        stage('Build Docker Image') {
            steps {
                bat "docker build -t %IMAGE% ."
            }
        }
 
        stage('Create Network') {
            steps {
                bat "docker network inspect %NETWORK% >nul 2>&1 || docker network create %NETWORK%"
            }
        }
 
        stage('Start MySQL') {
            steps {
                bat """
                docker rm -f %MYSQL_CONT% 2>nul
 
                docker run -d --name %MYSQL_CONT% --network %NETWORK% ^
                    -e MYSQL_ROOT_PASSWORD=%MYSQL_PWD% ^
                    -e MYSQL_DATABASE=%MYSQL_DB% ^
                    -p 3312:3306 ^
                    -v google-classroom-mysql-data-jenkins:/var/lib/mysql ^
                    mysql:8.0
                """
            }
        }
 
        stage('Wait for MySQL (HEALTHCHECK equivalent)') {
            steps {
                bat """
                echo Waiting for MySQL to be ready...
 
                :loop
                docker exec %MYSQL_CONT% mysqladmin ping -h localhost -uroot -p%MYSQL_PWD% >nul 2>&1
 
                IF ERRORLEVEL 1 (
                    timeout /t 5 >nul
                    goto loop
                )
 
                echo MySQL is ready!
                """
            }
        }
 
        stage('Run API') {
            steps {
                bat """
                docker rm -f %API_CONT% 2>nul
 
                docker run -d --name %API_CONT% --network %NETWORK% ^
                    -e ASPNETCORE_ENVIRONMENT=Development ^
                    -e ASPNETCORE_URLS=http://+:8080 ^
                    -e ConnectionStrings__DefaultConnection="Server=%MYSQL_CONT%;Port=3306;Database=%MYSQL_DB%;User=root;Password=%MYSQL_PWD%;" ^
                    -e JwtSettings__Issuer=GoogleClassroomAPI ^
                    -e JwtSettings__Audience=GoogleClassroomUsers ^
                    -e JwtSettings__Secret=super_secret_key_that_should_be_long_enough_for_hmacsha256 ^
                    -e JwtSettings__AccessTokenExpirationMinutes=60 ^
                    -e JwtSettings__RefreshTokenExpirationDays=7 ^
                    -p 5086:8080 ^
                    -v google-classroom-api-media-jenkins:/app/SimpleStorage ^
                    %IMAGE%
                """
            }
        }
 
    }
}