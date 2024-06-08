<template>
    <div v-if="isLoaded" class="main">
        <h1>{{ tour.name }}</h1>
        <p class="tourDescription">{{ tour.description }}</p>
        <h5>{{ tour.startDate }} --- {{ tour.endDate }}</h5>
        <img :src="tour.imageUrl" alt="tour image" class="tourImage">


        <div style="margin-top: 50px;">
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
                    <p>{{ destination.hotel.description }}</p>
                    <img :src="destination.hotel.imageUrl" alt="hotel image" class="hotelImage">
                </div>
            </div>
        </div>

        <div style="margin-top: 50px; display: flex; flex-direction: column;">
            <button>Buy</button>
            <button>Reserve</button>
            <button>Update</button>
        </div>
    </div>
</template>

<script>
import * as tourAPI from '@/services/API/tourAPI';
import * as dateHelper from '@/js/dateHelper';

export default {
    data() {
        return {
            isLoaded: false,
            tour: null
        }
    },
    methods: {
    },

    async mounted() {
        this.tour = await tourAPI.getTourById(this.$route.params.id);
        this.tour.startDate = await dateHelper.formatDate(this.tour.startDate);
        this.tour.endDate = await dateHelper.formatDate(this.tour.endDate);
        for (let destination of this.tour.destinations) {
            destination.startDate = await dateHelper.formatDate(destination.startDate);
            destination.endDate = await dateHelper.formatDate(destination.endDate);
        }
        this.isLoaded = true;
    }
}
</script>

<style>
@import "./../assets/css/styleTourPage.css";
</style>