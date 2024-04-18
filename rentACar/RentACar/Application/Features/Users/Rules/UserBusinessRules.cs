using Application.Features.Auth.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;
using Core.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user is null)
            throw new BusinessException(AuthMessages.UserDontExists);
        return Task.CompletedTask;
    }
    public async Task UserIdShouldBeExistsWhenSelected(int id)
    {
        User? result = await _userRepository.GetAsync(predicate: b => b.Id == id, enableTracking: false);
        if (result == null)
            throw new BusinessException(AuthMessages.UserDontExists);
    }

    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);
        return Task.CompletedTask;
    }

    public async Task UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: e => e.Email == email, enableTracking: false);
        if (doesExist)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
        


    public async Task UserEmailShouldNotExistsWhenUpdate(int id, string email)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: e => e.Email == email && e.Id !=id, enableTracking: false);
        if (doesExist)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
}