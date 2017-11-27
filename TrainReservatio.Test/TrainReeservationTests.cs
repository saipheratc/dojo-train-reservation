using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrainReservation.Contract;
using TrainReservation.Domain;
using Moq;
using TrainReservation.DTO;
using System.Collections.Generic;

namespace TrainReservatio.Test
{
    [TestClass]
    public class TrainReeservationTests
    {
        //sut
        private ITicketOffice _sut;

        //Mocks
        Mock<ITrainRepository> _repositoryMock;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<ITrainRepository>();

            _sut = new TicketOffice(_repositoryMock.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_exception_if_destination_not_found()
        {
            //arrange
            var request = new ReservationRequest
            {
                Destination = "BAHIA"
            };

            //act
            _sut.MakeReservation(request);

        }

        [TestMethod]
        public void Should_return_reservation_response_if_destination_is_valid_filled_with_destination_equal_state()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", It.IsAny<DateTime>()))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 4
                                }
                            });

            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("MATO GROSSO", It.IsAny<DateTime>()))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "MATO GROSSO",
                                    SeatsInCoach = 4
                                }
                            });

            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("RIO DE JANEIRO", It.IsAny<DateTime>()))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "RIO DE JANEIRO",
                                    SeatsInCoach = 4
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01),
                Passengers = new string[0]
            };

            var request2 = new ReservationRequest
            {
                Destination = "MATO GROSSO",
                ScheduledDate = new DateTime(2017, 01, 01),
                Passengers = new string[0]
            };

            var request3 = new ReservationRequest
            {
                Destination = "RIO DE JANEIRO",
                ScheduledDate = new DateTime(2017, 01, 01),
                Passengers = new string[0]
            };

            //act
            var response = _sut.MakeReservation(request);
            var response2 = _sut.MakeReservation(request2);
            var response3 = _sut.MakeReservation(request3);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual("BAHIA", response.Destination);

            Assert.IsNotNull(response2);
            Assert.AreEqual("MATO GROSSO", response2.Destination);

            Assert.IsNotNull(response3);
            Assert.AreEqual("RIO DE JANEIRO", response3.Destination);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void deve_lancar_exceçao_se_o_destino_for_encontrado_e_data_nao_agendada()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "bahia",
                                    SeatsInCoach = 4
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 12, 0, 0)
            };

            //act
            _sut.MakeReservation(request);

        }

        [TestMethod]
        public void Deve_retornar_data_da_reserva_igual_data_enviada_no_request_quando_trem_estiver_disponivel()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 4,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 12, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 4,
                                    ScheduledDate = new DateTime(2017, 01, 01, 12, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new string[0]
            };

            var request2 = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 12, 0, 0),
                Passengers = new string[0]
            };

            //act
            var response = _sut.MakeReservation(request);

            var response2 = _sut.MakeReservation(request2);

            //arrange
            Assert.IsNotNull(response);
            Assert.IsNotNull(response2);
            Assert.AreEqual(new DateTime(2017, 01, 01, 11, 0, 0), response.ScheduledDate);
            Assert.AreEqual(new DateTime(2017, 01, 01, 12, 0, 0), response2.ScheduledDate);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Retornar_exception_se_nao_tiver_capacidade_disponivel_no_mesmo_vagao()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 1,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "Passageiro1", "Passageiro2" }
            };

            //act
            var response = _sut.MakeReservation(request);
        }

        [TestMethod]
        public void Retornar_reserva_para_cada_passageiro_quanto_houver_assentos_disponíveis()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 5,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "Passageiro1", "Passageiro2" }
            };

            var request2 = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "Passageiro1", "Passageiro2", "Passageiro3" }
            };

            //act
            var response = _sut.MakeReservation(request);

            var response2 = _sut.MakeReservation(request2);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Seats.Length);

            Assert.IsNotNull(response2);
            Assert.AreEqual(3, response2.Seats.Length);
        }

        /// <summary>
        /// Verifica se ao requisitar uma reserva cujo numero de passageiros seja superior a capacidade do primeiro trem, esta reserva
        /// será alocada no segundo trem.
        /// </summary>
        [TestMethod]
        public void Verifica_se_aloca_os_passageiros_no_segundo_trem_quando_quantidade_de_assentos_do_primeiro_inferior_ao_request()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 1,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                },

                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 10,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "Passageiro1", "Passageiro2" }
            };

            //act
            var response = _sut.MakeReservation(request);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Seats.Length);

        }

        /// <summary>
        /// verifica se quando existe 3 trens disponiveis para o conjunto de destino/data e os dois primeiros não possuem capacidade
        /// capacidade para alocar a reserva e o terceiro possui, a reserva é feita no terceiro trem
        /// </summary>

        [TestMethod]
        public void Verifica_alocacao_com_3_trens_quando_2_nao_possuem_capacidade()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 1,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                },

                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 2,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                },
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 3,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }

                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "P1", "P2", "P3" }
            };

            //act
            var response = _sut.MakeReservation(request);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Seats.Length);

        }


        /// <summary>
        /// Verifica se todos os assentos disponibilizados na reserva são distintos
        /// Ex: A1, A2, A3 para 3 pessoas na reserva
        /// </summary>
        [TestMethod]
        public void Verifica_assentos_disponbilizados_para_passageiros()
        {
            //arrange
            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 3,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                },
                                new Train
                                {
                                    Id = Guid.NewGuid(),
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 5,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "P1", "P2", "P3" }
            };

            var request2 = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "P1", "P2", "P3", "P4", "P5" }
            };

            //act
            var response = _sut.MakeReservation(request);
            var response2 = _sut.MakeReservation(request2);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual("A1", response.Seats[0]);
            Assert.AreEqual("A2", response.Seats[1]);
            Assert.AreEqual("A3", response.Seats[2]);

            Assert.IsNotNull(response2);
            Assert.AreEqual("A1", response2.Seats[0]);
            Assert.AreEqual("A2", response2.Seats[1]);
            Assert.AreEqual("A3", response2.Seats[2]);
            Assert.AreEqual("A4", response2.Seats[3]);
            Assert.AreEqual("A5", response2.Seats[4]);
        }

        [TestMethod]
        public void Verifica_se_os_assentos_foram_alocados_no_trem_correto()
        {
            //arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            _repositoryMock.Setup(x => x.GetTrainByDestinationAndDate("BAHIA", new DateTime(2017, 01, 01, 11, 0, 0)))
                            .Returns(new List<Train>
                            {
                                new Train
                                {
                                    Id = id1,
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 3,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                },
                                new Train
                                {
                                    Id = id2,
                                    Coachs = 10,
                                    Destination = "BAHIA",
                                    SeatsInCoach = 5,
                                    ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0)
                                }
                            });

            var request = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "P1", "P2", "P3" }
            };

            var request2 = new ReservationRequest
            {
                Destination = "BAHIA",
                ScheduledDate = new DateTime(2017, 01, 01, 11, 0, 0),
                Passengers = new[] { "P1", "P2", "P3","P4","P5" }
            };

            //act
            var response = _sut.MakeReservation(request);
            var response2 = _sut.MakeReservation(request2);

            //assert
            Assert.IsNotNull(response);
            Assert.AreEqual(id1, response.TrainId);

            //assert
            Assert.IsNotNull(response2);
            Assert.AreEqual(id2, response2.TrainId);
        }
    }
}
