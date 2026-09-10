using System;
using System.Collections.Generic;
using System.Linq;
using eraasoftTask13.Models;

namespace eraasoftTask13.Data
{
    public static class DbSeeder
    {
        public static void SeedData(CinemaDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {
                        Name = "Sci-Fi",
                        Description = "Science fiction films exploring futuristic concepts, space, time, and technology.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/280px-PNG_transparency_demonstration_1.png",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Category {
                        Name = "Action",
                        Description = "High-octane films featuring stunts, fights, chases, and explosive sequences.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Action_camera.jpg/320px-Action_camera.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Category {
                        Name = "Drama",
                        Description = "Character-driven stories exploring human emotions and complex relationships.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/Lol_k.jpg/240px-Lol_k.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Category {
                        Name = "Thriller",
                        Description = "Suspenseful films designed to excite and keep audiences on edge.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8e/Suspense.jpg/240px-Suspense.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Category {
                        Name = "Comedy",
                        Description = "Light-hearted films designed to entertain and make audiences laugh.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/Camponotus_flavomarginatus_ant.jpg/320px-Camponotus_flavomarginatus_ant.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Cinemas.Any())
            {
                var cinemas = new List<Cinema>
                {
                    new Cinema {
                        Name = "Grand Cairo Cinema",
                        Description = "The premier cinema experience in the heart of Cairo.",
                        City = "Cairo",
                        Address = "5 Talaat Harb Street, Downtown Cairo",
                        Phone = "+20 2 2391 0000",
                        Email = "info@grandcairo.com",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f9/CairoEgypt.jpg/320px-CairoEgypt.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Cinema {
                        Name = "Alexandria Riviera Cinema",
                        Description = "Stunning cinema on the beautiful Alexandria waterfront.",
                        City = "Alexandria",
                        Address = "Corniche El Nil, Stanley, Alexandria",
                        Phone = "+20 3 5481 1234",
                        Email = "contact@riviera-alex.com",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/41/Alexandria_qaitbay.jpg/320px-Alexandria_qaitbay.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    }
                };
                context.Cinemas.AddRange(cinemas);
                context.SaveChanges();
            }

            if (!context.Halls.Any())
            {
                var c1 = context.Cinemas.First(c => c.Name == "Grand Cairo Cinema");
                var c2 = context.Cinemas.First(c => c.Name == "Alexandria Riviera Cinema");

                var halls = new List<Hall>
                {
                    new Hall { Name = "Hall A - Main",     CinemaId = c1.Id, Capacity = 50, IsActive = true },
                    new Hall { Name = "Hall B - Standard", CinemaId = c1.Id, Capacity = 30, IsActive = true },
                    new Hall { Name = "VIP Lounge",        CinemaId = c2.Id, Capacity = 20, IsActive = true }
                };
                context.Halls.AddRange(halls);
                context.SaveChanges();
            }

            if (!context.Seats.Any())
            {
                var seats = new List<Seat>();
                var halls = context.Halls.ToList();
                foreach (var hall in halls)
                {
                    int rows = hall.Capacity / 10;
                    for (int r = 1; r <= rows; r++)
                    {
                        for (int s = 1; s <= 10; s++)
                        {
                            seats.Add(new Seat
                            {
                                HallId = hall.Id,
                                RowNumber = r,
                                SeatNumber = s,
                                SeatType = hall.Name.Contains("VIP") ? SeatType.VIP : SeatType.Regular,
                                IsAvailable = true
                            });
                        }
                    }
                }
                context.Seats.AddRange(seats);
                context.SaveChanges();
            }

            if (!context.Actors.Any())
            {
                var actors = new List<Actor>
                {
                    new Actor {
                        Name = "Leonardo DiCaprio",
                        Bio = "Academy Award-winning American actor and film producer known for his roles in Titanic, Inception, The Revenant, and The Wolf of Wall Street.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Leonardo_DiCaprio_2010.jpg/220px-Leonardo_DiCaprio_2010.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Actor {
                        Name = "Tom Hardy",
                        Bio = "British actor known for his intense and transformative performances in Mad Max: Fury Road, Venom, The Dark Knight Rises, and Dunkirk.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Tom_Hardy_by_Gage_Skidmore.jpg/220px-Tom_Hardy_by_Gage_Skidmore.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Actor {
                        Name = "Timothée Chalamet",
                        Bio = "Academy Award-nominated American-French actor known for his roles in Call Me by Your Name, Dune, Interstellar, and Little Women.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Timoth%C3%A9e_Chalamet_2018.jpg/220px-Timoth%C3%A9e_Chalamet_2018.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Actor {
                        Name = "Cillian Murphy",
                        Bio = "Irish actor best known for playing Tommy Shelby in Peaky Blinders and J. Robert Oppenheimer in the 2023 biographical film, for which he won an Academy Award.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Cillian_Murphy_2018.jpg/220px-Cillian_Murphy_2018.jpg",
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    }
                };
                context.Actors.AddRange(actors);
                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                var sciFiCat = context.Categories.First(c => c.Name == "Sci-Fi");
                var thrillCat = context.Categories.First(c => c.Name == "Thriller");
                var cairoCinema = context.Cinemas.First(c => c.Name == "Grand Cairo Cinema");
                var alexCinema = context.Cinemas.First(c => c.Name == "Alexandria Riviera Cinema");

                var movies = new List<Movie>
                {
                    new Movie {
                        Name = "Inception",
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O. A mind-bending thriller by Christopher Nolan.",
                        Status = MovieStatus.Ended,
                        ReleaseDate = new DateTime(2010, 7, 16),
                        DurationInMinutes = 148,
                        MainImage = "https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg",
                        CategoryId = sciFiCat.Id,
                        CinemaId = cairoCinema.Id,
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Movie {
                        Name = "Dune: Part Two",
                        Description = "Paul Atreides unites with Chani and the Fremen while seeking revenge against the conspirators who destroyed his family. Facing a choice between the love of his life and the fate of the universe.",
                        Status = MovieStatus.NowShowing,
                        ReleaseDate = new DateTime(2024, 3, 1),
                        DurationInMinutes = 166,
                        MainImage = "https://upload.wikimedia.org/wikipedia/en/8/8d/Dune_Part_Two_film_poster.jpg",
                        CategoryId = sciFiCat.Id,
                        CinemaId = cairoCinema.Id,
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Movie {
                        Name = "Oppenheimer",
                        Description = "The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb during World War II. Directed by Christopher Nolan.",
                        Status = MovieStatus.NowShowing,
                        ReleaseDate = new DateTime(2023, 7, 21),
                        DurationInMinutes = 180,
                        MainImage = "https://upload.wikimedia.org/wikipedia/en/4/4a/Oppenheimer_%28film%29.jpg",
                        CategoryId = thrillCat.Id,
                        CinemaId = alexCinema.Id,
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Movie {
                        Name = "Mad Max: Fury Road",
                        Description = "In a post-apocalyptic wasteland, a woman rebels against a tyrannical ruler in search for her homeland with the aid of a group of female prisoners, a psychotic worshiper, and a drifter named Max.",
                        Status = MovieStatus.Ended,
                        ReleaseDate = new DateTime(2015, 5, 15),
                        DurationInMinutes = 120,
                        MainImage = "https://upload.wikimedia.org/wikipedia/en/6/6e/Mad_Max_Fury_Road.jpg",
                        CategoryId = context.Categories.First(c => c.Name == "Action").Id,
                        CinemaId = alexCinema.Id,
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    },
                    new Movie {
                        Name = "Interstellar",
                        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Directed by Christopher Nolan, starring Matthew McConaughey.",
                        Status = MovieStatus.ComingSoon,
                        ReleaseDate = new DateTime(2025, 6, 1),
                        DurationInMinutes = 169,
                        MainImage = "https://upload.wikimedia.org/wikipedia/en/b/bc/Interstellar_film_poster.jpg",
                        CategoryId = sciFiCat.Id,
                        CinemaId = cairoCinema.Id,
                        IsActive = true, CreatedAt = DateTime.UtcNow
                    }
                };
                context.Movies.AddRange(movies);
                context.SaveChanges();

                // Assign actors to movies
                var inception = context.Movies.First(m => m.Name == "Inception");
                var dune = context.Movies.First(m => m.Name == "Dune: Part Two");
                var oppenheimer = context.Movies.First(m => m.Name == "Oppenheimer");
                var madMax = context.Movies.First(m => m.Name == "Mad Max: Fury Road");
                var interstellar = context.Movies.First(m => m.Name == "Interstellar");

                var dicaprio = context.Actors.First(a => a.Name == "Leonardo DiCaprio");
                var hardy = context.Actors.First(a => a.Name == "Tom Hardy");
                var chalamet = context.Actors.First(a => a.Name == "Timothée Chalamet");
                var murphy = context.Actors.First(a => a.Name == "Cillian Murphy");

                context.MovieActors.AddRange(
                    new MovieActor { MovieId = inception.Id, ActorId = dicaprio.Id },
                    new MovieActor { MovieId = dune.Id, ActorId = chalamet.Id },
                    new MovieActor { MovieId = oppenheimer.Id, ActorId = murphy.Id },
                    new MovieActor { MovieId = madMax.Id, ActorId = hardy.Id },
                    new MovieActor { MovieId = interstellar.Id, ActorId = dicaprio.Id },
                    new MovieActor { MovieId = interstellar.Id, ActorId = murphy.Id }
                );

                // Add sample gallery images for Inception
                context.MovieImages.AddRange(
                    new MovieImage { MovieId = inception.Id, ImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Stellarised_city.jpg/320px-Stellarised_city.jpg", IsMain = false, DisplayOrder = 1 },
                    new MovieImage { MovieId = dune.Id, ImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/88/ArabiaSaudita_Rub_al_Khali.jpg/320px-ArabiaSaudita_Rub_al_Khali.jpg", IsMain = false, DisplayOrder = 1 }
                );

                context.SaveChanges();
            }

            if (!context.ShowTimes.Any())
            {
                var nowShowingMovie = context.Movies.FirstOrDefault(m => m.Status == MovieStatus.NowShowing);
                if (nowShowingMovie != null)
                {
                    var cinId = nowShowingMovie.CinemaId;
                    var hId = context.Halls.First(h => h.CinemaId == cinId).Id;

                    var shows = new List<ShowTime>
                    {
                        new ShowTime {
                            MovieId = nowShowingMovie.Id, CinemaId = cinId, HallId = hId,
                            StartTime = DateTime.Now.AddDays(1).Date.AddHours(19),
                            EndTime = DateTime.Now.AddDays(1).Date.AddHours(22),
                            TicketPrice = 150m, IsActive = true, CreatedAt = DateTime.UtcNow
                        },
                        new ShowTime {
                            MovieId = nowShowingMovie.Id, CinemaId = cinId, HallId = hId,
                            StartTime = DateTime.Now.AddDays(2).Date.AddHours(21),
                            EndTime = DateTime.Now.AddDays(3).Date.AddHours(0),
                            TicketPrice = 180m, IsActive = true, CreatedAt = DateTime.UtcNow
                        }
                    };
                    context.ShowTimes.AddRange(shows);
                    context.SaveChanges();
                }
            }
        }
    }
}
