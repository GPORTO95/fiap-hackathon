using Hackathon.HealthMed.Doctors.Application.Abstractions.Data;
using Hackathon.HealthMed.Doctors.Domain.Doctors;
using Hackathon.HealthMed.Kernel.Messaging;
using Hackathon.HealthMed.Kernel.Shared;

namespace Hackathon.HealthMed.Doctors.Application.Doctors.ListPaged;

public sealed record ListPagedDoctorQuery(int Page = 1, int PageSize = 10, string? Search = null, Specialty? Specialty = null) : IQuery<PagedList<ListPagedDoctorQueryResponse>>;

public sealed record ListPagedDoctorQueryResponse(
    Guid DoctorId,
    string Name,
    string Email,
    string Crm,
    string Cpf,
    Specialty Specialty);

internal sealed class ListWithAvailableSchedulesDoctorQueryHandler(
    IApplicationDbContext context) : IQueryHandler<ListPagedDoctorQuery, PagedList<ListPagedDoctorQueryResponse>>
{
    public async Task<Result<PagedList<ListPagedDoctorQueryResponse>>> Handle(ListPagedDoctorQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Doctor> doctorsQuery = context.Doctors;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            doctorsQuery = doctorsQuery.Where(d =>
                d.Name.Value.Contains(request.Search) ||
                d.Email.Value.Contains(request.Search) ||
                d.Crm.Value.Contains(request.Search) ||
                d.Cpf.Value.Contains(request.Search));
        }

        if (request.Specialty.HasValue)
        {
            doctorsQuery = doctorsQuery.Where(d => d.Specialty == request.Specialty);
        }

        var doctorsResponseQuery = doctorsQuery
            .Select(e => new ListPagedDoctorQueryResponse(
                e.Id,
                e.Name.Value,
                e.Email.Value,
                e.Crm.Value,
                e.Cpf.Value,
                e.Specialty));

        return await PagedList<ListPagedDoctorQueryResponse>.CreateAsync(doctorsResponseQuery, request.Page, request.PageSize);
    }
}