
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Domain.Doctors;

public static class DoctorScheduleErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "DoctorSchedule.NotFound",
        "Doctor schedule not found.");

    public static readonly Error IsNotFree = Error.Conflict(
        "DoctorSchedule.IsNotFree",
        "Doctor schedule is not free.");

    public static readonly Error Denied = Error.Problem(
        "DoctorSchedule.Denied",
        "Doctor schedule is denied for update.");

    public static readonly Error IsNotPending = Error.Problem(
        "DoctorSchedule.IsNotPending",
        "Doctor schedule is not pending.");
}
