export const environment = {
    env: process.env['NODE_ENV'],
    server: process.env['NODE_ENV'] == 'docker' ? process.env['SERVER'] : 'http://localhost:5160',
    stripePublishKey: 'pk_test_51OIGBKKfdlsNCGTnyxFs1IzyDJ1Wfe4TKOpDgeDyyubqHixilJu2an4WBdktNWgAUqfPMV6fw8eLNjf6QumdqC9X00g6whFvLS'
};