using Labs.DataAccess.Contracts;
using Labs.DataAccess.EFModels;
using Labs.DataAccess.Models;

namespace Labs.DataAccess.Repositories
{
    public class LinqDestinationRepository : IRepository<Destinations>
    {
        public (bool created, string errorMessage) Create(Destinations entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    context.Destinations.Add(new Destination()
                    {
                        DestinationName = entity.DestinationName
                    });
                    context.SaveChanges();
                    result.created = true;
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }

        public (bool deleted, string errorMessage) Delete(int id)
        {
            var result = (deleted: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var deletionDestination = context.Destinations.FirstOrDefault(d => d.Id == id);

                    if (deletionDestination == null)
                    {
                        result.errorMessage = "Destination with given id not exists";
                    }
                    else
                    {
                        context.Destinations.Remove(deletionDestination);
                        context.SaveChanges();
                        result.deleted = true;
                    }
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }

        public Destinations Get(int id)
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var deletionDestination = context.Destinations.FirstOrDefault(d => d.Id == id);

                    return deletionDestination == null ? null : new Destinations()
                    {
                        Id = deletionDestination.Id,
                        DestinationName = deletionDestination.DestinationName
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<Destinations> GetAll()
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var deletionDestination = context.Destinations.Select(x => new Destinations()
                    {
                        Id = x.Id,
                        DestinationName = x.DestinationName
                    }).ToList();

                    return deletionDestination;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public (bool updated, string errorMessage) Update(Destinations entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var updating = context.Destinations.FirstOrDefault(x => x.Id == entity.Id);

                    if (updating == null)
                    {
                        result.errorMessage = "Destination with given id not exists";
                    }
                    else
                    {
                        updating.DestinationName = entity.DestinationName;
                        context.SaveChanges();
                        result.updated = true;
                    }
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }
    }
}
