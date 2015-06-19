using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace PostInterface
{
    class DataBase
    {
        static PostDataBaseContext Context;

        public static class Areas
        {
            public static List<Area> GetAllAreas()
            {
                List<Area> res = new List<Area>();
                res.Add(new Area() { Name = "(не выбрано)" });
                res.AddRange(Context.Area.ToList());
                return res;
            }

            public static Area GetAreaById(long ID_Area)
            {
                Area res = Context.Area.Where("it.ID_Area = @ID_Area", new ObjectParameter("ID_Area", ID_Area)).ToList()[0];
                return res;
            }

            public static void AddArea(Area a)
            {
                Context.Area.AddObject(a);
                Context.SaveChanges();
            }

            public static void EditArea(Area a)
            {
                Area ToEdit = GetAreaById(a.ID_Area);
                ToEdit.Name = a.Name;
                Context.SaveChanges();
            }
        }

        public static class Regions
        {
            public static List<Region> GetAllRegionsByAreaId(int ID_Area)
            {
                List<Region> res = new List<Region>();
                res.Add(new Region() { Name = "(не выбрано)" });
                res.AddRange(Context.Region.Where("it.ID_Area = @ID_Area", new ObjectParameter("ID_Area", ID_Area)).ToList());
                return res;
            }

            public static Region GetRegionById(long ID_Region)
            {
                Region res = Context.Region.Where("it.ID_Region = @ID_Region", new ObjectParameter("ID_Region", ID_Region)).ToList()[0];
                return res;
            }

            public static void AddRegion(Region r)
            {
                Context.Region.AddObject(r);
                Context.SaveChanges();
            }

            public static void EditRegion(Region r)
            {
                Region ToEdit = GetRegionById(r.ID_Region);
                ToEdit.Name = r.Name;
                Context.SaveChanges();
            }
        }

        public static class Cities
        {
            public static List<City> GetAllCitiesByRegionId(int ID_Region = 0)
            {
                List<City> res = new List<City>();
                res.Add(new City() { Name = "(не выбрано)" });
                if (ID_Region != 0)
                    res.AddRange(Context.City.Where("it.ID_Region = @ID_Region", new ObjectParameter("ID_Region", ID_Region)).ToList());
                else res.AddRange(Context.City.ToList());
                for (int i = 1; i < res.Count; i++)
                {
                    res[i] = new City() { ID_City = res[i].ID_City, ID_City_Type = res[i].ID_City_Type, Name = res[i].Name };
                    res[i].Name = Context.City_Type.Where("it.ID_City_Type = @ID_City_Type", new ObjectParameter("ID_City_Type", res[i].ID_City_Type)).ToList()[0].Name + " " + res[i].Name;
                }
                return res;
            }

            public static City GetCityById(long ID_City)
            {
                City tmp = Context.City.Where("it.ID_City = @ID_City", new ObjectParameter("ID_City", ID_City)).ToList()[0];
                City res = new City() { ID_City = tmp.ID_City, ID_City_Type = tmp.ID_City_Type, ID_Region = tmp.ID_Region };
                res.Name = Context.City_Type.Where("it.ID_City_Type = @ID_City_Type", new ObjectParameter("ID_City_Type", tmp.ID_City_Type)).ToList()[0].Name + " " + tmp.Name;
                return res;
            }

            public static List<City_Type> GetAllCityTypes()
            {
                List<City_Type> res = new List<City_Type>();
                res.Add(new City_Type() { Name = "(не выбрано)" });
                res.AddRange(Context.City_Type.ToList());
                return res;
            }

            public static City_Type GetCityTypeById(long ID_City_Type)
            {
                City_Type res = Context.City_Type.Where("it.ID_City_Type = @ID_City_Type", new ObjectParameter("ID_City_Type", ID_City_Type)).ToList()[0];
                return res;
            }

            public static void AddCity(City c)
            {
                Context.City.AddObject(c);
                Context.SaveChanges();
            }

            public static void EditCity(City c)
            {
                City ToEdit = GetCityById(c.ID_City);
                ToEdit.Name = c.Name;
                Context.SaveChanges();
            }
        }

        public static class Streets
        {
            public static List<Street> GetAllStreetsByCityId(int ID_City)
            {
                List<Street> res = new List<Street>();
                res.Add(new Street() { Name = "(не выбрано)" });
                res.AddRange(Context.Street.Where("it.ID_City = @ID_City", new ObjectParameter("ID_City", ID_City)).ToList());
                for (int i = 1; i < res.Count; i++)
                {
                    res[i] = new Street() { ID_Street = res[i].ID_Street, ID_Street_Type = res[i].ID_Street_Type, Name = res[i].Name };
                    res[i].Name = Context.Street_Type.Where("it.ID_Street_Type = @ID_Street_Type", new ObjectParameter("ID_Street_Type", res[i].ID_Street_Type)).ToList()[0].Name + " " + res[i].Name;
                }
                return res;
            }

            public static List<Street_Type> GetAllStreetTypes()
            {
                List<Street_Type> res = new List<Street_Type>();
                res.Add(new Street_Type() { Name = "(не выбрано)" });
                res.AddRange(Context.Street_Type.ToList());
                return res;
            }

            public static Street_Type GetStreetTypeById(long ID_Street_Type)
            {
                Street_Type res = Context.Street_Type.Where("it.ID_Street_Type = @ID_Street_Type", new ObjectParameter("ID_Street_Type", ID_Street_Type)).ToList()[0];
                return res;
            }

            public static Street GetStreetById(long ID_Street)
            {
                Street res = Context.Street.Where("it.ID_Street = @ID_Street", new ObjectParameter("ID_Street", ID_Street)).ToList()[0];
                return res;
            }

            public static void AddStreet(Street s)
            {
                Context.Street.AddObject(s);
                Context.SaveChanges();
            }

            public static void EditStreet(Street s)
            {
                Street ToEdit = GetStreetById(s.ID_Street);
                ToEdit.Name = s.Name;
                Context.SaveChanges();
            }
        }

        public static class Workers
        {
            public static List<Worker> GetAllWorkers()
            {
                List<Worker> res = new List<Worker>();
                res.Add(new Worker() { FIO = "(не выбрано)" });
                res.AddRange(Context.Worker.ToList());
                return res;
            }

            public static List<Worker> GetWorkersWithoutRemoved()
            {
                List<Worker> res = new List<Worker>();
                res.Add(new Worker() { FIO = "(не выбрано)" });
                res.AddRange(Context.Worker.Where("it.ID_Office <> 0").ToList());
                return res;
            }

            public static Worker GetWorkerByLoginAndPassword(string Login, string Password)
            {
                var tmp = Context.Worker.Where("it.Login = @Login AND it.Password = @Password",
                                                new ObjectParameter("Login", Login),
                                                new ObjectParameter("Password", Password));
                if (tmp.ToList().Count == 0)
                    return null;
                else return tmp.ToList()[0];
            }

            public static Worker GetWorkerByID(long ID_Worker)
            {
                var tmp = Context.Worker.Where("it.ID_Worker = @ID_Worker", new ObjectParameter("ID_Worker", ID_Worker));
                if (tmp.ToList().Count == 0)
                    return null;
                else return tmp.ToList()[0];
            }

            public static void AddWorker(Worker w)
            {
                Context.Worker.AddObject(w);
                Context.SaveChanges();
            }

            public static void EditWorker(Worker UpdatedWorker)
            {
                Worker w = GetWorkerByID(UpdatedWorker.ID_Worker);
                w.ID_Office = UpdatedWorker.ID_Office;
                w.Login = UpdatedWorker.Login;
                w.Password = UpdatedWorker.Password;
                w.FIO = UpdatedWorker.FIO;
                w.Passport_Serie = UpdatedWorker.Passport_Serie;
                w.Passport_Number = UpdatedWorker.Passport_Number;
                Context.SaveChanges();
            }

            public static void DeleteWorker(long ID_Worker)
            {
                Worker ToDelete = GetWorkerByID(ID_Worker);
                ToDelete.ID_Office = 0;
                Context.SaveChanges();
            }

            public static bool IsFreeLogin(string Login)
            {
                var tmp = Context.Worker.Where("it.Login = @Login AND it.ID_Office <> 0",
                                                new ObjectParameter("Login", Login));
                if (tmp.ToList().Count == 0)
                    return true;
                else return false;
            }
        }

        public static class Officies
        {
            public static List<Office> GetAllOfficies()
            {
                List<Office> res = new List<Office>();
                res.Add(new Office() { Name = "(не выбрано)" });
                res.AddRange(Context.Office.ToList());
                return res;
            }

            public static Office GetOfficeByID(long ID_Office)
            {
                Office res = Context.Office.Where("it.ID_Office = @ID_Office", new ObjectParameter("ID_Office", ID_Office)).ToList()[0];
                return res;
            }
        }

        public static class Statuses
        {
            public static List<Letter_Status> GetAllLetterStatuses()
            {
                List<Letter_Status> res = new List<Letter_Status>();
                res.Add(new Letter_Status() { Status = "(не выбрано)" });
                res.AddRange(Context.Letter_Status.ToList());
                return res;
            }

            public static Letter_Status GetLetterStatusById(long ID_Letter_Status)
            {
                Letter_Status res = Context.Letter_Status.Where("it.ID_Letter_Status = @ID_Letter_Status", new ObjectParameter("ID_Letter_Status", ID_Letter_Status)).ToList()[0];
                return res;
            }

            public static List<Full_Letter_Status> GetAllFullLetterStatusesByLetterStatusId(long ID_Letter_Status)
            {
                List<Full_Letter_Status> res = new List<Full_Letter_Status>();
                res.Add(new Full_Letter_Status() { Full_Status = "(не выбрано)" });
                res.AddRange(Context.Full_Letter_Status.Where("it.ID_Letter_Status = @ID_Letter_Status", new ObjectParameter("ID_Letter_Status", ID_Letter_Status)).ToList());
                return res;
            }

            public static Full_Letter_Status GetFullLetterStatusById(long ID_Full_Letter_Status)
            {
                Full_Letter_Status res = Context.Full_Letter_Status.Where("it.ID_Full_Letter_Status = @ID_Full_Letter_Status", new ObjectParameter("ID_Full_Letter_Status", ID_Full_Letter_Status)).ToList()[0];
                return res;
            }
        }

        public static class Consigment_Types
        {
            public static List<Consignment_Type> GetAllConsigmentTypes()
            {
                List<Consignment_Type> res = new List<Consignment_Type>();
                res.Add(new Consignment_Type() { Type = "(не выбрано)" });
                res.AddRange(Context.Consignment_Type.ToList());
                return res;
            }

            public static Consignment_Type GetConsigmentTypeById(long ID_Consignment_Type)
            {
                Consignment_Type res = Context.Consignment_Type.Where("it.ID_Consignment_Type = @ID_Consignment_Type", new ObjectParameter("ID_Consignment_Type", ID_Consignment_Type)).ToList()[0];
                return res;
            }
        }

        public static class Consigments
        {
            public static Consignment GetConsigmentById(long ID_Consignment)
            {
                var tmp = Context.Consignment.Where("it.ID_Consignment = @ID_Consignment", new ObjectParameter("ID_Consignment", ID_Consignment)).ToList();
                Consignment res = null;
                if (tmp.Count != 0)
                    res = tmp[0];
                return res;
            }

            public static void AddConsignment(Consignment Cons)
            {
                Context.Consignment.AddObject(Cons);
                Context.SaveChanges();
            }

            public static void EditConsigment(Consignment Cons)
            {
                Consignment ToEdit = GetConsigmentById(Cons.ID_Consignment);
                ToEdit.ID_Consignment_Type = Cons.ID_Consignment_Type;
                ToEdit.ID_Courier = Cons.ID_Courier;
                ToEdit.ID_Office_Worker = Cons.ID_Office_Worker;
                ToEdit.ID_Reciever = Cons.ID_Reciever;
                ToEdit.ID_Sender = Cons.ID_Sender;
                ToEdit.ID_Transport_Company = Cons.ID_Transport_Company;
                ToEdit.Total_Cost = Cons.Total_Cost;
                ToEdit.Weight = Cons.Weight;
                ToEdit.Worth = Cons.Worth;
                Context.SaveChanges();

            }

            public static void DeleteConsigment(long ID_Consigment)
            {
                Context.DeleteObject(GetConsigmentById(ID_Consigment));
                Context.SaveChanges();
            }
        }

        public static class Post_Officies
        {
            public static List<Post_Office> GetAllPostOfficies()
            {
                List<Post_Office> res = new List<Post_Office>();
                res.Add(new Post_Office() { Post_Index = 0 });
                res.AddRange(Context.Post_Office.ToList());
                return res;
            }

            public static Post_Office GetPostOfficeByPostIndex(long Post_Index)
            {
                var tmp = Context.Post_Office.Where("it.Post_Index = @Post_Index", new ObjectParameter("Post_Index", Post_Index)).ToList();
                if (tmp.Count == 0)
                    return null;
                return tmp[0];
            }

            public static void AddPostOffice(Post_Office PO)
            {
                Context.Post_Office.AddObject(PO);
                Context.SaveChanges();
            }

            public static void DeletePostOffice(long Post_Index)
            {
                Context.DeleteObject(GetPostOfficeByPostIndex(Post_Index));
                Context.SaveChanges();
            }
        }

        public static class Transport_Companies
        {
            public static List<Transport_Company> GetAllCompanies()
            {
                List<Transport_Company> res = new List<Transport_Company>();
                res.Add(new Transport_Company() { Name = "(не выбрано)" });
                res.AddRange(Context.Transport_Company.ToList());
                return res;
            }

            public static Transport_Company GetCompanyById(long ID_Transport_Company)
            {
                Transport_Company res = Context.Transport_Company.Where("it.ID_Transport_Company = @ID_Transport_Company", new ObjectParameter("ID_Transport_Company", ID_Transport_Company)).ToList()[0];
                return res;
            }

            public static void AddCompany(Transport_Company TC)
            {
                Context.Transport_Company.AddObject(TC);
                Context.SaveChanges();
            }

            public static void EditCompany(Transport_Company EditedCompany)
            {
                Transport_Company ToEdit = GetCompanyById(EditedCompany.ID_Transport_Company);
                ToEdit.Name = EditedCompany.Name;
                Context.SaveChanges();
            }

            public static void DeleteCompany(long ID_Transport_Company)
            {
                Context.DeleteObject(GetCompanyById(ID_Transport_Company));
                Context.SaveChanges();
            }
        }

        public static class Clients
        {
            public static Client GetClientById(long ID_Client)
            {
                Client res = Context.Client.Where("it.ID_Client = @ID_Client", new ObjectParameter("ID_Client", ID_Client)).ToList()[0];
                return res;
            }

            public static void AddClient(Client c)
            {
                Context.Client.AddObject(c);
                Context.SaveChanges();
            }

            public static void EditClient(Client Edited)
            {
                Client toEdit = GetClientById(Edited.ID_Client);
                toEdit.Building = Edited.Building;
                toEdit.Flat = Edited.Flat;
                toEdit.House_Number = Edited.House_Number;
                toEdit.ID_Street = Edited.ID_Street;
                toEdit.Index = Edited.Index;
                toEdit.Name = Edited.Name;
                Context.SaveChanges();
            }

            public static void DeleteClient(long ID_Client)
            {
                Context.DeleteObject(GetClientById(ID_Client));
                Context.SaveChanges();
            }
        }

        public static class Track_Elems
        {
            public static void AddTrackElem(Track_List t)
            {
                Context.Track_List.AddObject(t);
                Context.SaveChanges();
            }

            public static List<TrackElemClass> GetTrackListByConsigmentId(long ID_Consignment)
            {
                List<TrackElemClass> res = new List<TrackElemClass>();
                var realTrackList = Context.Track_List.Where("it.ID_Consignment = @ID_Consignment", new ObjectParameter("ID_Consignment", ID_Consignment)).ToList();
                foreach (Track_List t in realTrackList)
                {
                    TrackElemClass tmp = new TrackElemClass();
                    tmp.DateTime = t.Date;
                    Full_Letter_Status FLS = Statuses.GetFullLetterStatusById(t.ID_Full_Letter_Status);
                    tmp.FullStatus = FLS.Full_Status;
                    tmp.Status = Statuses.GetLetterStatusById(FLS.ID_Letter_Status).Status;
                    tmp.PostIndex = t.Post_Index;
                    tmp.ID_Letter = t.ID_Consignment;
                    tmp.Сity = Cities.GetCityById(Post_Officies.GetPostOfficeByPostIndex(t.Post_Index).ID_City).Name;
                    res.Add(tmp);
                }
                return res;
            }

            public static void DeleteConsignmentFromTrackList(long ID_Consignment)
            {
                var ToDelete = Context.Track_List.Where("it.ID_Consignment = @ID_Consignment", new ObjectParameter("ID_Consignment", ID_Consignment)).ToList();
                foreach (Track_List t in ToDelete)
                {
                    Context.DeleteObject(t);
                }
                Context.SaveChanges();
            }
        }

        public static class Transport_Costs
        {
            public static void EditTransportCost(Transport_Cost TC)
            {
                var tmp = Context.Transport_Cost.Where("it.ID_Transport_Company = @ID_Transport_Company AND ((it.ID_City_From = @ID_City_From AND it.ID_City_To = @ID_City_To) OR (it.ID_City_From = @ID_City_To AND it.ID_City_To = @ID_City_From))",
                                                new ObjectParameter("ID_Transport_Company", TC.ID_Transport_Company),
                                                new ObjectParameter("ID_City_From", TC.ID_City_From),
                                                new ObjectParameter("ID_City_To", TC.ID_City_To)).ToList();
                if (tmp.Count == 0)
                    Context.Transport_Cost.AddObject(TC);
                else
                {
                    Transport_Cost ToEdit = tmp[0];
                    Context.DeleteObject(tmp[0]);
                    Context.Transport_Cost.AddObject(TC);
                }
                Context.SaveChanges();
            }

            public static long GetTransportCost(long ID_Transport_Conpany, long ID_City_From, long ID_City_To)
            {
                var tmp = Context.Transport_Cost.Where("it.ID_Transport_Company = @ID_Transport_Company AND ((it.ID_City_From = @ID_City_From AND it.ID_City_To = @ID_City_To) OR (it.ID_City_From = @ID_City_To AND it.ID_City_To = @ID_City_From))",
                                                new ObjectParameter("ID_Transport_Company", ID_Transport_Conpany),
                                                new ObjectParameter("ID_City_From", ID_City_From),
                                                new ObjectParameter("ID_City_To", ID_City_To)).ToList();
                if (tmp.Count == 0)
                    return ID_Transport_Conpany;
                else
                {
                    return tmp[0].Cost;
                }
            }
        }

        public static void Init()
        {
            Context = new PostDataBaseContext();
        }
    }
}
