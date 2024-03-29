﻿using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDTO> GetUsersChargingHistory(int userId, int page, int pageSize, string search);
        Task<EventResponseDTO> GetFullChargingHistory(int page, int pageSize, string search);
        Task<EventResponseDTO> StartCharging(EventChargingDTO eventDTO);
        Task<EventResponseDTO> EndCharging(EventChargingDTO eventDTO);
        Task<EventResponseDTO> GetStatistics(int year, int month, int chargerId);
    }
}
