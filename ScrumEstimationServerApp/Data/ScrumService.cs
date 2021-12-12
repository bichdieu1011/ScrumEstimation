using ScrumEstimationServerApp.Entity;

namespace ScrumEstimationServerApp.Data
{
    public class ScrumService
    {
        private readonly ILogger<ScrumService> _logger;
        readonly DbEntity dbEntity;
        public ScrumService(ILogger<ScrumService> logger, DbEntity dbEntity)
        {
            _logger = logger;
            this.dbEntity = dbEntity;
        }

        public string CreateEstimationRoom(string roomUrl)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");

            dbEntity.Room.Add(new Room
            {
                Id = guid,
                Name = roomUrl,
            });
            dbEntity.SaveChanges();
            return guid;
        }


        public string GetRoomName(string id)
        {
            return dbEntity.Room.FirstOrDefault(s => s.Id == id)?.Name ?? id;
        }

        public List<RoomModel> GetRooms(string[] id)
        {
            return dbEntity.Room.Where(s => id.Contains(s.Id)).Select(s => new RoomModel { Id = s.Id, Name = string.IsNullOrEmpty(s.Name) ? s.Id : s.Name }).ToList();
        }
        public async Task JoinRoom(string id, string Username)
        {
            var attendee = dbEntity.Attendee.FirstOrDefault(s => s.Room == id && s.username == Username);
            if (attendee == null)
            {
                dbEntity.Attendee.Add(new Attendee
                {
                    Room = id,
                    username = Username,
                    IsActive = true
                });
            }
            else
            {
                attendee.IsActive = true;
            }
            dbEntity.SaveChanges();
        }

        public async Task<long> AddTicket(string id, string ticketname)
        {
            var ticketAdd = dbEntity.Ticket.FirstOrDefault(s => s.Room == id && s.Name == ticketname);
            if (ticketAdd == null)
            {
                ticketAdd = new Ticket
                {
                    Room = id,
                    Name = ticketname,
                    IsPick = false
                };
                dbEntity.Ticket.Add(ticketAdd);
            }


            dbEntity.SaveChanges();

            return ticketAdd.Id;
        }

        public async Task PickTicket(string id, string ticketname)
        {
            var currentTicket = dbEntity.Ticket.FirstOrDefault(s => s.Room == id && s.Name == ticketname);
            if (currentTicket == null)
            {
                return;
            }
            var tickets = dbEntity.Ticket.Where(s => s.Room == id);
            foreach (var ticket in tickets)
            {
                ticket.IsPick = ticket.Name == ticketname;
                ticket.Point = ticket.Name == ticketname ? null : ticket.Point;
            }

            var estimates = dbEntity.Estimation.Where(s => s.ticketId == currentTicket.Id);
            foreach (var estimate in estimates)
            {
                estimate.point = (decimal?)null;
            }

            dbEntity.SaveChanges();
        }

        public IEnumerable<TicketModel> GetTickets(string id)
        {
            var listItem = dbEntity.Ticket.Where(s => s.Room == id).Select(s => new TicketModel
            {
                Id = s.Id,
                Point = s.Point,
                TicketName = s.Name,
                IsPick = s.IsPick
            }).AsEnumerable();

            return listItem;
        }

        public IEnumerable<UserModel> getUsers(string id)
        {
            var currentTicket = dbEntity.Ticket.FirstOrDefault(s => s.Room == id && s.IsPick);
            if (currentTicket == null)
            {
                return dbEntity.Attendee.Where(s => s.Room == id && s.IsActive)
               .Select(s => new UserModel
               {
                   UserName = s.username,
                   IsEstimate = false
               }).AsEnumerable();
            }

            var pickedUserIds = dbEntity.Estimation.Where(s => s.ticketId == currentTicket.Id && s.point != null).Select(s => s.userId);
            var listItem = dbEntity.Attendee.Where(s => s.Room == id && s.IsActive) 
                .Select(s => new UserModel
                {
                    UserName = s.username,
                    IsEstimate = pickedUserIds.Contains(s.Id)
                }).AsEnumerable();

            return listItem;
        }

        public void RemoveUser(string id, string username)
        {
            var item = dbEntity.Attendee.FirstOrDefault(s => s.Room == id && username == s.username);
            if(item != null)
            {
                item.IsActive = false;
            }
            dbEntity.SaveChanges();
        }

        public void Estimate(string roomId, long ticketId, string username, decimal point)
        {
            var user = dbEntity.Attendee.FirstOrDefault(s => s.username == username && s.Room == roomId);
            if (user != null)
            {
                var estimate = dbEntity.Estimation.FirstOrDefault(s => s.userId == user.Id && s.ticketId == ticketId);
                if (estimate != null)
                {
                    estimate.point = point;
                }
                else
                {
                    dbEntity.Estimation.Add(new Estimation
                    {
                        point = point,
                        ticketId = ticketId,
                        userId = user.Id

                    });
                }
                dbEntity.SaveChanges();
            }
        }

        public decimal? Estimated(string roomId, long ticketId)
        {
            var point = (decimal?)0;
            var ticket = dbEntity.Ticket.FirstOrDefault(s => s.Room == roomId && s.Id == ticketId);
            if (ticket != null)
            {
                point = dbEntity.Estimation.Where(s => s.ticketId == ticketId && s.point != null).Select(s => s.point).AsEnumerable().Average();
                ticket.Point = point;
                dbEntity.SaveChanges();
            }
            return point;
        }

        public EstimatedInfo EtimatedInfo(long ticketId, string roomId)
        {
            var result = new EstimatedInfo();
            result.Point = dbEntity.Ticket.FirstOrDefault(s => s.Id == ticketId)?.Point;
            result.EstimatedPoints = dbEntity.Estimation.Where(s => s.ticketId == ticketId)
                                            .Join(dbEntity.Attendee.Where(s => s.Room == roomId && s.IsActive),
                                                    s1 => s1.userId,
                                                    s2 => s2.Id,
                                                    (s1, s2) => new UserModel
                                                    {
                                                        Point = s1.point,
                                                        UserName = s2.username
                                                    }).ToList();

            return result;
        }
    }
}
