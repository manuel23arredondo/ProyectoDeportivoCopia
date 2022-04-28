namespace PrimerProyectoClubDeportivoPA2.Web.Data
{
    using Microsoft.AspNetCore.Identity;
    using PrimerProyectoClubDeportivoPA2.Web.Helpers;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Data.Entities;
    public class Seeder
    {
        private readonly DataContext dataContext;
        private readonly IUserHelper userHelper;

        public Seeder(DataContext dataContext, IUserHelper userHelper)
        {
            this.dataContext = dataContext;
            this.userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await dataContext.Database.EnsureCreatedAsync();
            await userHelper.CheckRoleAsync("Admin");
            await userHelper.CheckRoleAsync("Coach");
            await userHelper.CheckRoleAsync("Member");

            if (!this.dataContext.Admins.Any())
            {
                var user = await CheckUser("Bill", "Gates", "272 560 8922", DateTime.Now, 1000000, "bill.microsoft@gmail.com", "123456");
                await CheckAdmin(user, "Admin", "$12000");
            }

            if (!this.dataContext.Coaches.Any())
            {
                var user = await CheckUser("Armando", "Lopez", "272 115 0000", DateTime.Now, 2001515, "armando.l@gmail.com", "123456");
                await CheckCoach(user, "Coach", "$5000", "Boxeador profesional", "XXX");
                user = await CheckUser("Antonio", "Ruiz", "272 155 1000", DateTime.Now, 2001555, "tony.rl@gmail.com", "123456");
                await CheckCoach(user, "Coach", "$8500", "Clavadista profesional", "XXX");
                user = await CheckUser("Pedro", "Cortés", "272 895 0560", DateTime.Now, 2005555, "cortes.peter@gmail.com", "123456");
                await CheckCoach(user, "Coach", "$4200", "Tenista", "XXX");
            }

            if (!this.dataContext.MembershipTypes.Any())
            {
                await CheckMembershipType("Plata", "Te permite el acceso a los deportes: Fútbol, Basketball y Zumba", "$1500");
                await CheckMembershipType("Oro", "Incluye todos los deportes de la membresía Plata además de Karate, Ping Pong y Atletismo", "$2500");
                await CheckMembershipType("Platino", "Incluye todos los deportes de la membresía Oro además de Box, Natación, Clavados, Tennis y Golf", "$3500");
            }

            if (!this.dataContext.Members.Any())
            {
                var user = await CheckUser("Luis", "Perez", "272 862 4156", DateTime.Now, 1005858, "perez.luis@gmail.com", "123456");
                var membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Platino");
                await CheckMember(user, "Member", membershipType, "XXX");
                
                user = await CheckUser("Samantha", "Lopez", "272 555 4666", DateTime.Now, 1001180, "samy@gmail.com", "123456");
                await CheckMember(user, "Member", membershipType, "XXX");

                user = await CheckUser("Porfirio", "Ruiz", "272 887 7531", DateTime.Now, 1001059, "porfirito@gmail.com", "123456");
                await CheckMember(user, "Member", membershipType, "XXX");

                membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Oro");
                user = await CheckUser("Rodrigo", "Armendariz", "272 100 8566", DateTime.Now, 1008555, "armendariz.rod@gmail.com", "123456");
                await CheckMember(user, "Member", membershipType, "XXX");

                membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Plata");
                user = await CheckUser("Renato", "Ibarra", "272 851 7444", DateTime.Now, 1005744, "ibarra.rena@gmail.com", "123456");
                await CheckMember(user, "Member", membershipType, "XXX");
            }

            if (!this.dataContext.WeekDays.Any())
            {
                await CheckWeekday("Lunes");
                await CheckWeekday("Martes");
                await CheckWeekday("Miércoles");
                await CheckWeekday("Jueves");
                await CheckWeekday("Viernes");
                await CheckWeekday("Sábado");
                await CheckWeekday("Domingo");
            }
            
            if (!this.dataContext.Sports.Any())
            {
                await CheckSport("Fútbol");
                await CheckSport("Basketball");
                await CheckSport("Natación");
                await CheckSport("Clavados");
                await CheckSport("Zumba");
                await CheckSport("Baseball");
                await CheckSport("Golf");
                await CheckSport("Karate");
                await CheckSport("Ping Pong");
                await CheckSport("Tennis");
                await CheckSport("Box");
                await CheckSport("Atletismo");
            }

            if (!this.dataContext.Facilities.Any())
            {
                await CheckFacility("Alberca olímpica", "Cuenta con 8 carriles, tiene una profundidad de 2.7m y calefacción", "AO001", "XXX");
                await CheckFacility("Alberca de clavados", "Cuenta con plataformas de clavados olímpicos tiene una profundidad de 5.4m, calefacción y jacuzzi", "AC002", "XXX");
                await CheckFacility("Cancha de Tenis (Arcilla)", "Recientemente inagurada cuenta con gradas alrededor y videoarbitraje", "CT001", "XXX");
                await CheckFacility("Cancha de Tenis (Pasto)", "Cuenta con gradas alrededor y videoarbitraje", "CT002", "XXX");
                await CheckFacility("Campo de Golf", "Cuenta con 18 hoyos, fosa de arena y carritos de golf", "CG003", "XXX");
                await CheckFacility("Campo de Baseball", "Cuenta con gradas para 200 personas, bancas de local y visitante y marcador digital", "CB004", "XXX");
                await CheckFacility("Campo de Fútbol", "Cuenta con gradas para 300 personas, vestidores, regaderas y sauna", "CF005", "XXX");
                await CheckFacility("Cancha de Basketball", "Cuenta con vestidores, tableros de acrílico y duela de madera", "CB006", "XXX");
                await CheckFacility("Explanada", "Cuenta con techo retraíble y sonido envolvente", "EX001", "XXX");
                await CheckFacility("Explanada-2", "Igualmente cuenta con techo retraíble y con tatami al centro", "EX002", "XXX");
                await CheckFacility("Gimnasio", "Contamos con +50 equipos para ejercitarse, zona de cardio, zona de pesas y ring de boxeo", "GM001","XXX");
                await CheckFacility("Campo multiusos", "Cuenta con pista de atletismo de tartán con 6 carriles", "CM007", "XXX");
            }

            if (!this.dataContext.Schedules.Any())
            {
                var weekDay = dataContext.WeekDays.FirstOrDefault(c => c.Name == "Lunes");
                var facility = dataContext.Facilities.FirstOrDefault(c => c.Name == "Gimnasio");
                await CheckSchedule(DateTime.Now, DateTime.Now, weekDay, facility);

                weekDay = dataContext.WeekDays.FirstOrDefault(c => c.Name == "Jueves");
                await CheckSchedule(DateTime.Now, DateTime.Now, weekDay, facility);

                facility = dataContext.Facilities.FirstOrDefault(c => c.Name == "Alberca de clavados");
                await CheckSchedule(DateTime.Now, DateTime.Now, weekDay, facility);

                weekDay = dataContext.WeekDays.FirstOrDefault(c => c.Name == "Domingo");
                facility = dataContext.Facilities.FirstOrDefault(c => c.Name == "Explanada");
                await CheckSchedule(DateTime.Now, DateTime.Now, weekDay, facility);
            }

            if (!this.dataContext.TrainingSessions.Any())
            {
                var coach = dataContext.Coaches.FirstOrDefault(c => c.Id == 1);
                var sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Box");
                var schedule = dataContext.Schedules.FirstOrDefault(c => c.Id == 1);
                await CheckTrainingSession("Boxeo avanzado", coach, sport, schedule, "10");

                await CheckTrainingSession("Boxeo principiante", coach, sport, schedule, "10");

                coach = dataContext.Coaches.FirstOrDefault(c => c.Id == 2);
                sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Clavados");
                schedule = dataContext.Schedules.FirstOrDefault(c => c.Id == 3);
                await CheckTrainingSession("Clavados intermedio", coach, sport, schedule, "20");
            }

            if (!this.dataContext.Permits.Any())
            {
                var sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Golf");
                var membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Platino");
                await CheckPermit("Permiso platino", sport, membershipType);

                sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Clavados");
                await CheckPermit("Permiso platino", sport, membershipType);

                sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Zumba");
                membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Plata");
                await CheckPermit("Permiso plata", sport, membershipType);

                sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Ping Pong");
                membershipType = dataContext.MembershipTypes.FirstOrDefault(c => c.Name == "Oro");
                await CheckPermit("Permiso oro", sport, membershipType);
            }

            if (!this.dataContext.AdditionalSkills.Any())
            {
                var coach = dataContext.Coaches.FirstOrDefault(c => c.Id == 1);
                var sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Box");
                await CheckAdditionalSkill("Judo cinta negra", coach, sport);

                coach = dataContext.Coaches.FirstOrDefault(c => c.Id == 2);
                sport = dataContext.Sports.FirstOrDefault(c => c.Name == "Clavados");
                await CheckAdditionalSkill("Ex-jugador waterpolo", coach, sport);
            }

            if (!this.dataContext.Agendas.Any())
            {
                var member = dataContext.Members.FirstOrDefault(c => c.Id == 1);
                var trainingSession = dataContext.TrainingSessions.FirstOrDefault(c => c.Name == "Boxeo avanzado");
                await CheckAgenda(member, trainingSession);

                member = dataContext.Members.FirstOrDefault(c => c.Id == 2);
                trainingSession = dataContext.TrainingSessions.FirstOrDefault(c => c.Name == "Boxeo principiante");
                await CheckAgenda(member, trainingSession);

                member = dataContext.Members.FirstOrDefault(c => c.Id == 3);
                trainingSession = dataContext.TrainingSessions.FirstOrDefault(c => c.Name == "Clavados intermedio");
                await CheckAgenda(member, trainingSession);
            }
        }

        private async Task<User> CheckUser(string firstName, string lastName, string phoneNumber, DateTime birthdate, int enrollmentNumber, string email, string password)
        {
            var user = await userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    BirhtDate = birthdate,
                    EnrollmentNumber = enrollmentNumber,
                    Email = email,
                    UserName = email
                };
                var result = await userHelper.AddUserAsync(user, password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Error no se pudo crear el usuario");
                }
            }
            return user;
        }

        private async Task CheckAdmin(User user, string rol, string salary)
        {
            this.dataContext.Admins.Add(new Admin 
            { 
                User = user,
                Salary = salary
            });
            await this.dataContext.SaveChangesAsync();
            await userHelper.AddUserToRoleAsync(user, rol);
        }

        private async Task CheckCoach(User user, string rol, string salary, string expertise, string imageUrl)
        {
            this.dataContext.Coaches.Add(new Coach 
            {
                User = user,
                Salary = salary,
                Expertise = expertise,
                ImageUrl = imageUrl
            });
            await this.dataContext.SaveChangesAsync();
            await userHelper.AddUserToRoleAsync(user, rol);
        }

        private async Task CheckMember(User user, string rol, MembershipType membershipType, string imageUrl)
        {
            this.dataContext.Members.Add(new Member 
            { 
                User = user,
                MembershipType = membershipType,
                ImageUrl = imageUrl
            });
            await this.dataContext.SaveChangesAsync();
            await userHelper.AddUserToRoleAsync(user, rol);
        }

        private async Task CheckWeekday(string name)
        {
            this.dataContext.WeekDays.Add(new WeekDay { Name = name });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckSport(string name)
        {
            this.dataContext.Sports.Add(new Sport { Name = name });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckFacility(string name, string description, string code, string imageUrl)
        {
            this.dataContext.Facilities.Add(new Facility
            {
                Name = name,
                Description = description,
                Code = code,
                ImageUrl = imageUrl
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckMembershipType(string name, string description, string cost)
        {
            this.dataContext.MembershipTypes.Add(new MembershipType
            {
                Name = name,
                Description = description,
                Cost = cost
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckPermit(string name, Sport sport, MembershipType membershipType)
        {
            this.dataContext.Permits.Add(new Permit
            {
                Name = name,
                Sport = sport,
                MembershipType = membershipType
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckSchedule(DateTime startingHour, DateTime finishingHour, WeekDay weekDay, Facility facility)
        {
            this.dataContext.Schedules.Add(new Schedule
            {
                StartingHour = startingHour,
                FinishingHour = finishingHour,
                WeekDay = weekDay,
                Facility = facility
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckTrainingSession(string name, Coach coach, Sport sport, Schedule schedule, string capacity)
        {
            this.dataContext.TrainingSessions.Add(new TrainingSession
            {
                Name = name,
                Coach = coach,
                Sport = sport,
                Schedule = schedule,
                Capacity = capacity
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckAgenda(Member member, TrainingSession trainingSession)
        {
            this.dataContext.Agendas.Add(new Agenda
            {
                Member = member,
                TrainingSession = trainingSession
            });
            await this.dataContext.SaveChangesAsync();
        }

        private async Task CheckAdditionalSkill(string name, Coach coach, Sport sport)
        {
            this.dataContext.AdditionalSkills.Add(new AdditionalSkill
            {
                Name = name,
                Coach = coach,
                Sport = sport
            });
            await this.dataContext.SaveChangesAsync();
        }
    }
}
