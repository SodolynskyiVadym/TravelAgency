import { loadStripe } from '@stripe/stripe-js';
import axios from "axios";
import router from "@/services/router";

const mainUrl = "http://localhost:5113/pay";
let stripePromise;

const initializeStripe = async () => {
    stripePromise = await loadStripe("pk_test_51OIGBKKfdlsNCGTnyxFs1IzyDJ1Wfe4TKOpDgeDyyubqHixilJu2an4WBdktNWgAUqfPMV6fw8eLNjf6QumdqC9X00g6whFvLS");
}

initializeStripe();

export const reserveTour = async (paymentData, token) => {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        const id = await axios.post(`${mainUrl}/reserveTour`, paymentData, config).then((res) => res.data);

        const stripe = await stripePromise;
        await stripe.redirectToCheckout({
            sessionId: id
        });

    } catch (error) {
        router.push("/error");
    }
};



export async function getPaymentsByUser(token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.get(`${mainUrl}/getUserPayments`, config).then((res) => res.data);
    } catch (error) {
        router.push("/error");
    }
}



export const payExistingPayment = async (sessionStripeId) => {
    try {

        const stripe = await stripePromise;
        await stripe.redirectToCheckout({
            sessionId: sessionStripeId
        });

    } catch (error) {
        router.push("/error");
    }
}


export async function haveUserPayment(tourId, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.get(`${mainUrl}/haveUserPayment/${tourId}`, config).then((res) => res.data);
    } catch (error) {
        router.push("/error");
    }
}