using System;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Auth;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto model, [FromServices] IEmailService emailService, [FromServices] Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            if (string.IsNullOrEmpty(model.Email)) return BadRequest(new { message = "Email is required" });

            // Generate a 6 digit random code
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();

            // Save to cache for 10 minutes
            cache.Set($"otp_{model.Email}", otp, TimeSpan.FromMinutes(10));

            // Send Email in background so the UI doesn't hang waiting for Google SMTP
            var subject = "Google Classroom Clone - Verification Code";
            var body = $"<h3>Your verification code is: {otp}</h3><p>Use this code to verify your email address.</p>";

            // We MUST use a new scope, otherwise the request scope is disposed
            // We MUST use SuppressFlow, otherwise Kestrel keeps the Keep-Alive connection open
            var serviceScopeFactory = HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();
            var emailStr = model.Email; // capture string

            using (System.Threading.ExecutionContext.SuppressFlow())
            {
                _ = Task.Run(async () => 
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var scopedEmailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    try {
                        await scopedEmailService.SendEmailAsync(emailStr, subject, body);
                    } catch { } // Ignore errors in background
                });
            }

            return Ok(new { message = "OTP sent successfully" });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto model, [FromServices] Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Otp))
                return BadRequest(new { message = "Email and OTP are required" });

            if (cache.TryGetValue($"otp_{model.Email}", out string? savedOtp))
            {
                if (savedOtp == model.Otp)
                {
                    // Success! Remove from cache so it can't be reused
                    cache.Remove($"otp_{model.Email}");
                    return Ok(new { message = "OTP verified successfully" });
                }
            }

            return BadRequest(new { message = "Invalid or expired OTP" });
        }
    }
}
