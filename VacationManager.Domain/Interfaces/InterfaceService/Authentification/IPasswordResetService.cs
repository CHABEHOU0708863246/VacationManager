

namespace VacationManager.Domain.Interfaces.InterfaceService.Authentification
{
    public interface IPasswordResetService
    {
        Task<bool> ForgotPasswordAsync(string email, CancellationToken cancellationToken);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken);
    }
}
