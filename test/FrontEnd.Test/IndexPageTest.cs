using ConferenceDTO.Response;
using FrontEnd.Pages;
using FrontEnd.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FrontEnd.Test
{
    public class IndexPageTest
    {
        [Fact]
        public async Task Get()
        {
            var mockApiClient = new Mock<IApiClient>();
            mockApiClient.Setup(x => x.GetSessionsAsync()).ReturnsAsync(
                new List<SessionResponse>
                {
                    new SessionResponse
                    {
                        ID = 1,
                        ConferenceID = 1,
                        Title = "Keynote: Are There Any Questions?",
                        Slug = "keynote-are-there-any-questions",
                        Abstract = "Not this time. At NDC Oslo 2017, we're going to turn things upside-down and do the questions right at the beginning. Because, for as long as human beings have existed, we've asked questions. Questions about the world around us. Questions about the past, about the future, about our place in the universe. As our world moves online, the search for answers has become inextricably linked with the history, and the future, of software development. The earliest mechanical calculating machines were built to answer questions - to solve complex mathematical problems many thousands of times faster than their human operators.\r\nToday, in the age of connected devices and machine learning, we live in a world where humans ask questions and expect computers to have the answers. So let's take this chance to ask some of the really big questions: Who are we? How did we get here? How is modern software changing the way we interact with the world around us? Where are the really big questions in modern software development - and who's working on them? And how will those questions shape the future of our relationship with the machines that have become such an important part of our lives?",
                        StartTime = DateTimeOffset.Parse("2017-06-14T09:00:00+01:00"),
                        EndTime = DateTimeOffset.Parse("2017-06-14T10:00:00+01:00"),
                        Tags = new ConferenceDTO.Tag[]{ },
                        TrackId = 1,

                        Track = new ConferenceDTO.Track
                        {
                            TrackID = 1,
                            ConferenceID = 0,
                            Name = "Expo"
                        },
                        Speakers = new ConferenceDTO.Speaker[]
                        {
                            new ConferenceDTO.Speaker
                            {
                                ID = 111,
                                Name = "Dylan Beattie",                       
                            }
                        }
                    },
                    new SessionResponse
                    {
                        ID = 172,
                        ConferenceID = 1,
                        Title = "What To Expect When You Are Elixiring",
                        Slug = "what-to-expect-when-you-are-elixiring",
                        StartTime = DateTimeOffset.Parse("2017-06-15T10:20:00+01:00"),
                        EndTime = DateTimeOffset.Parse("2017-06-15T11:20:00+01:00"),
                        Tags = new ConferenceDTO.Tag[]{ },
                        TrackId = 5,

                        Track = new ConferenceDTO.Track
                        {
                            TrackID = 5,
                            ConferenceID = 0,
                            Name = "Room 4"
                        },
                        Speakers = new ConferenceDTO.Speaker[]
                        {
                            new ConferenceDTO.Speaker
                            {
                                ID = 140,
                                Name = "Johnny Winn",
                            }
                        }
                    }

                });

            var pageModel = new AgendaModel(mockApiClient.Object);
            await pageModel.OnGetAsync("keynote-are-there-any-questions");

            Assert.Equal(2, pageModel.DayOffsets.Count());
            Assert.Equal(DateTimeOffset.Parse("2017-06-14T09:00:00+01:00"), pageModel.Sessions.Single().Key);
            Assert.Equal(1, pageModel.Sessions.Single().Single().ID);
            Assert.Equal("Keynote: Are There Any Questions?", pageModel.Sessions.Single().Single().Title);

            await pageModel.OnGetAsync("what-to-expect-when-you-are-elixiring");

            Assert.Equal(2, pageModel.DayOffsets.Count());
            Assert.Equal(DateTimeOffset.Parse("2017-06-15T10:20:00+01:00"), pageModel.Sessions.Single().Key);
            Assert.Equal(172, pageModel.Sessions.Single().Single().ID);
            Assert.Equal("What To Expect When You Are Elixiring", pageModel.Sessions.Single().Single().Title);
        }
    }
}
