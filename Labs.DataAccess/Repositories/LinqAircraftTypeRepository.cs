using Labs.DataAccess.Contracts;
using Labs.DataAccess.EFModels;
using Labs.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.DataAccess.Repositories
{
    public class LinqAircraftTypeRepository : IRepository<AircraftTypes>
    {
        public (bool created, string errorMessage) Create(AircraftTypes entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    context.AircraftTypes.Add(new AircraftType()
                    {
                        AircraftTypeName = entity.AircraftTypeName
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
                    var deletionType = context.AircraftTypes.FirstOrDefault(d => d.Id == id);

                    if (deletionType == null)
                    {
                        result.errorMessage = "Aircraft type with given id not exists";
                    }
                    else
                    {
                        context.AircraftTypes.Remove(deletionType);
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

        public AircraftTypes Get(int id)
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var result = context.AircraftTypes.FirstOrDefault(d => d.Id == id);

                    return result == null ? null : new AircraftTypes()
                    {
                        Id = result.Id,
                        AircraftTypeName = result.AircraftTypeName
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<AircraftTypes> GetAll()
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var typesList = context.AircraftTypes.Select(x => new AircraftTypes()
                    {
                        Id = x.Id,
                        AircraftTypeName = x.AircraftTypeName
                    }).ToList();

                    return typesList;
                }
                catch
                {
                    return null;
                }
            }
        }

        public (bool updated, string errorMessage) Update(AircraftTypes entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var updating = context.AircraftTypes.FirstOrDefault(x => x.Id == entity.Id);

                    if (updating == null)
                    {
                        result.errorMessage = "Aircraft type with given id not exists";
                    }
                    else
                    {
                        updating.AircraftTypeName = entity.AircraftTypeName;
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
