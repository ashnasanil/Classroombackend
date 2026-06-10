pipeline {
 
    agent any
 
    environment {
        IMAGE = "google-classroom-api-jenkins:${BUILD_NUMBER}"
        NETWORK = "googleclassroom_default"
        MYSQL_CONT = "google-classroom-db"
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