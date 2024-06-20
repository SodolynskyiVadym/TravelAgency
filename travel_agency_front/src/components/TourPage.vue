<template>
    <div v-if="isLoaded" class="main">
        <h1>{{ tour.name }}</h1>
        <p class="tourDescription">{{ tour.description }}</p>
        <h5>{{ tour.startDate }} --- {{ tour.endDate }}</h5>
        <img :src="tour.imageUrl" alt="tour image" class="tourImage">


        <div>
            <div v-for="(destination, index) in tour.destinations" :key="index" class="location">
                <h2>{{ index + 1 }} location</h2>
                <h3>{{ destination.hotel.place.name }}({{ destination.hotel.place.country }})</h3>
                <h4>{{ destination.startDate }} --- {{ destination.endDate }}</h4>
                <p>{{ destination.hotel.place.description }}</p>
                <div class="locationImages">
                    <div v-for="imageUrl in destination.hotel.place.imagesUrls" :key="imageUrl.id">
                        <img style="width: 500px" :src="imageUrl.url" alt="location image">
                    </div>
                </div>
                <div class="hotel">
                    <h3>Your hotel({{ destination.hotel.place.name }})</h3>
                    <h4>"{{ destination.hotel.name }}"</h4>
                    <h4>Address: {{ destination.hotel.address }}</h4>
                    <p style="margin-top: 30px;">{{ destination.hotel.description }}</p>
                    <img :src="destination.hotel.imageUrl" alt="hotel image" class="hotelImage">
                </div>
            </div>
        </div>



        <div v-if="user.role">
            <div style="margin-top: 60px; text-align: center;">
                <h2>Quantity</h2>
                <input type="number" v-model="quantity" min="1" max="10" @input="calculatePrice">
            </div>
            <div>
                <h2 style="margin-top: 30px;">Price: {{ tour.price }}</h2>
            </div>

            <div style="margin-top: 30px; display: flex; flex-direction: column;">
                <button class="button-action" @click="reserveTour" v-if="!isSendRequest" :disabled="isSendRequest">Buy</button>
                <button class="button-action" @click="enterUpdatePage" v-if="user.role === 'EDITOR' || user.role === 'ADMIN'">Update</button>
            </div>
        </div>

    </div>
</template>

<script>
import * as tourAPI from '@/services/API/tourAPI';
import * as userAPI from '@/services/API/userAPI';
import * as dateHelper from '@/js/dateHelper';
import * as stripe from '@/js/stripe';

export default {
    data() {
        return {
            isLoaded: false,
            user: { role: "" },
            tour: null,
            quantity: 1,
            priceForOne: 0,
            isSendRequest: false
        }
    },
    methods: {
        enterUpdatePage() {
            this.$router.push(`/updateTour/${this.tour.id}`);
        },

        async reserveTour() {
            this.isSendRequest = true;
            const data = {
                tourId: this.tour.id,
                quantity: this.quantity
            }
            const token = localStorage.getItem('token');
            if (!token) {
                this.$router.push('/login');
            }

            await stripe.reserveTour(data, token);
        },

        async calculatePrice() {
            if (this.quantity < 1) {
                this.quantity = 1;
            }
            this.tour.price = this.priceForOne * this.quantity;
        }
    },

    async mounted() {
        this.tour = await tourAPI.getTourById(this.$route.params.id);
        this.priceForOne = this.tour.price;
        this.tour.startDate = await dateHelper.formatDate(this.tour.startDate);
        this.tour.endDate = await dateHelper.formatDate(this.tour.endDate);
        for (let destination of this.tour.destinations) {
            destination.startDate = await dateHelper.formatDate(destination.startDate);
            destination.endDate = await dateHelper.formatDate(destination.endDate);
        }

        const token = localStorage.getItem('token');
        if (token) {
            this.user = await userAPI.getUserByToken(token);
            if (!this.user) this.user = { role: "" };
        }

        this.isLoaded = true;
    }
}
</script>

<style>
@import "./../assets/css/styleTourPage.css";
</style>