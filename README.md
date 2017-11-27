Kata utilizado no Dojo do dia 27/11/2017

Extraído e adaptado do Livro "Coding Dojo Handbook - Emiliy Bache"


## Kata: Train Reservation
Railway operators aren't always known for their use of cutting edge technology, and in this case they're a little behind the times. The railway people want you to help them to improve their online booking service. They'd like to be able to not only sell tickets online, but to decide exactly which seats should be reserved, at the time of booking.

You're working on the "TicketOffice" application, and your next task is to implement the feature for reserving seats on a particular train.

You’re working on the “TicketOffice” class, and your next task is to implement the feature for reserving seats on a particular train. The TicketOffice class needs a method “MakeReservation” that will return a ReservationObject. It will take as argument a Reservation RequestObject, which contains all the needed information about what the customer wants, including which train they want to go on, and the number of seats they need. Your task is to write the code that takes a Reservation Request and finds suitable seats to reserve. You should return a Reservation object that lists the seats you have booked, and a booking reference. If it is not possible to find suitable seats to reserve, return an empty Reservation with no booking reference. You’ll also need to design away to store the information about which seats are reserved on which train. 


## Business Rules around Reservations
1. All the seats for one reservation must be in the same coach.
2. Train must have at least one coach
3. Coach must have at least eight seats
4. No more than 70% of seats may be reserved in advance, and ideally no individual coach should have no more than 70% reserved seats either.
4. The method responsible for the reservation must receive an
