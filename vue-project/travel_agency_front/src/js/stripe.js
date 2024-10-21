import { loadStripe } from '@stripe/stripe-js';
import axios from "axios";
import router from "@/services/router";

const mainUrl = "http://localhost:5113/pay";
const public_key = process.env.STRIPE_PUBLIC_KEY;
let stripePromise;

const initializeStripe = async () => {
    stripePromise = await loadStripe(public_key);
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