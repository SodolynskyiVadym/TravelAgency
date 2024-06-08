<template>
    <div v-if="isLoaded" class="main">
        <h1>{{ tour.name }}</h1>
        <p class="tourDescription">{{ tour.description }}</p>
        <h5>{{ tour.startDate }} --- {{ tour.endDate }}</h5>
        <img :src="tour.imageUrl" alt="tour image" class="tourImage">
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
        this.isLoaded = true;
    }
}
</script>

<style>
@import "./../assets/css/styleTourPage.css";
</style>