using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ScrumEstimationServerApp.Entity;
using ScrumEstimationServerApp.Helper;
using ScrumEstimationServerApp.Model;

namespace ScrumEstimationServerApp.Data
{
    public class ScrumService
    {
        private readonly ILogger<ScrumService> _logger;
        //private readonly DbEntity dbEntity;
        private IMemoryCache _memoryCache;
        private IConfiguration _configuration;
        string StoredFolder;
        public ScrumService(ILogger<ScrumService> logger, IMemoryCache _memoryCache, IConfiguration _configuration)
        {
            this._memoryCache = _memoryCache;
            _logger = logger;
            //this.dbEntity = dbEntity;
            this._configuration = _configuration;
            StoredFolder = _configuration["FileFolder"];
        }

        public string CreateEstimationRoom(string roomUrl)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");

            var newroom = new StoredFileModel()
            {
                Room = new Room
                {
                    Id = guid,
                    Name = roomUrl,
                },
                Attendees = new List<Attendee>(),
                Estimations = new List<Estimation>(),
                Tickets = new List<Ticket>()
            };
            //var value = JsonConvert.SerializeObject(newroom);
            //FileHelper.WriteFile($"{StoredFolder}/{guid}.json", value);
            _memoryCache.SetValue(guid, newroom);

            return guid;
        }

        public string GetRoomName(string id)
        {
            var detail = GetDetail(id);
            if (detail != null)
            {
                return detail.Room.Name;
            }
            return id;
        }

        public List<RoomModel> GetRooms(string[] id)
        {
            try
            {
                //foreach(var item in _memoryCache)
                //{

                //}
                //var listFile = Directory.EnumerateFiles(StoredFolder, ".json", SearchOption.AllDirectories).Where(s => id.Any(s1 => s.Contains(s1))).Select(s => new FileInfo(s)).ToList();
                //var rooms = new List<RoomModel>();
                //foreach (var file in listFile)
                //{
                //    StoredFileModel details;
                //    _memoryCache.TryGetValue(file.Name.Replace(".pdf", ""), out details);

                //    if (details != null)
                //        rooms.Add(new RoomModel
                //        {
                //            Id = details.Room.Id,
                //            Name = details.Room.Name,
                //        });
                //}

                //return rooms;
                return new List<RoomModel>();
            }
            catch (Exception ex)
            {
                return new List<RoomModel>();
            }
        }

        public async Task JoinRoom(string id, string Username)
        {
            var detail = GetDetail(id);
            var attendee = detail.Attendees.FirstOrDefault(s => s.Room == id && s.username == Username);
            if (attendee == null)
            {
                detail.Attendees.Add(new Attendee
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

            _memoryCache.SetValue(id, detail);
        }

        public async Task<long> AddTicket(string id, string ticketname)
        {
            var detail = GetDetail(id);
            var ticketAdd = detail.Tickets.FirstOrDefault(s => s.Room == id && s.Name == ticketname);
            if (ticketAdd == null)
            {
                ticketAdd = new Ticket
                {
                    Room = id,
                    Name = ticketname,
                    IsPick = false
                };
                detail.Tickets.Add(ticketAdd);
            }

            _memoryCache.SetValue(id, detail);

            return ticketAdd.Id;
        }

        public async Task PickTicket(string id, string ticketname)
        {
            var detail = GetDetail(id);
            var currentTicket = detail.Tickets.FirstOrDefault(s => s.Room == id && s.Name == ticketname);
            if (currentTicket == null)
            {
                return;
            }
            var tickets = detail.Tickets.Where(s => s.Room == id);
            foreach (var ticket in tickets)
            {
                ticket.IsPick = ticket.Name == ticketname;
                ticket.Point = ticket.Name == ticketname ? null : ticket.Point;
            }

            var estimates = detail.Estimations.Where(s => s.ticketId == currentTicket.Id);
            foreach (var estimate in estimates)
            {
                estimate.point = (decimal?)null;
            }

            _memoryCache.SetValue(id, detail);
        }

        public IEnumerable<TicketModel> GetTickets(string id)
        {
            var detail = GetDetail(id);
            var listItem = detail.Tickets.Where(s => s.Room == id).Select(s => new TicketModel
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
            var detail = GetDetail(id);
            var currentTicket = detail.Tickets.FirstOrDefault(s => s.Room == id && s.IsPick);
            if (currentTicket == null)
            {
                return detail.Attendees.Where(s => s.Room == id && s.IsActive)
               .Select(s => new UserModel
               {
                   UserName = s.username,
                   IsEstimate = false
               }).AsEnumerable();
            }

            var pickedUserIds = detail.Estimations.Where(s => s.ticketId == currentTicket.Id && s.point != null).Select(s => s.userId);
            var listItem = detail.Attendees.Where(s => s.Room == id && s.IsActive)
                .Select(s => new UserModel
                {
                    UserName = s.username,
                    IsEstimate = pickedUserIds.Contains(s.Id)
                }).AsEnumerable();

            return listItem;
        }

        public void RemoveUser(string id, string username)
        {
            var detail = GetDetail(id);
            var item = detail.Attendees.FirstOrDefault(s => s.Room == id && username == s.username);
            if (item != null)
            {
                item.IsActive = false;
            }
            _memoryCache.SetValue(id, detail);
        }

        public void Estimate(string roomId, long ticketId, string username, decimal point)
        {
            var detail = GetDetail(roomId);
            var user = detail.Attendees.FirstOrDefault(s => s.username == username && s.Room == roomId);
            if (user != null)
            {
                var estimate = detail.Estimations.FirstOrDefault(s => s.userId == user.Id && s.ticketId == ticketId);
                if (estimate != null)
                {
                    estimate.point = point;
                }
                else
                {
                    detail.Estimations.Add(new Estimation
                    {
                        point = point,
                        ticketId = ticketId,
                        userId = user.Id
                    });
                }
                _memoryCache.SetValue(roomId, detail);
            }
        }

        public decimal? Estimated(string roomId, long ticketId)
        {
            var detail = GetDetail(roomId);
            var point = (decimal?)0;
            var ticket = detail.Tickets.FirstOrDefault(s => s.Room == roomId && s.Id == ticketId);
            if (ticket != null)
            {
                point = detail.Estimations.Where(s => s.ticketId == ticketId && s.point != null).Select(s => s.point).AsEnumerable().Average();
                ticket.Point = point;
                _memoryCache.SetValue(roomId, detail);
            }
            return point;
        }

        public EstimatedInfo EtimatedInfo(long ticketId, string roomId)
        {
            var detail = GetDetail(roomId);
            var result = new EstimatedInfo();
            result.Point = detail.Tickets.FirstOrDefault(s => s.Id == ticketId)?.Point;
            result.EstimatedPoints = detail.Estimations.Where(s => s.ticketId == ticketId)
                                            .Join(detail.Attendees.Where(s => s.Room == roomId && s.IsActive),
                                                    s1 => s1.userId,
                                                    s2 => s2.Id,
                                                    (s1, s2) => new UserModel
                                                    {
                                                        Point = s1.point,
                                                        UserName = s2.username
                                                    }).ToList();

            return result;
        }

        StoredFileModel GetDetail(string id)
        {
            StoredFileModel detail;
            _memoryCache.TryGetValue(id, out detail);
            return detail ?? new StoredFileModel();
        }
    }
}