﻿using poc.core.api.net8.Enumerado;

namespace API.Admin.Domain.User.Events;

/// <summary>
/// Evento que represente um nov1 entidade.
/// </summary>
public class UserCreatedEvent : UserBaseEvent
{
    public UserCreatedEvent(Guid id, string firstName, string lastName, EGender gender, ENotificationType notification, string email, string phone, string password, List<string> role, DateTime dateOfBirth) :
        base(id, firstName, lastName, gender, notification, email, phone, password, role, dateOfBirth)
    {
    }
}
