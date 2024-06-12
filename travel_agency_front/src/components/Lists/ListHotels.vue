<template>
    <h1>Hotel's list</h1>
</template>

<script>
import * as hotelAPI from '@/services/API/hotelAPI';

export default {
    data() {
        return {
            hotels: []
        }
    },

    methods: {

    },

    async mounted() {
        this.hotels = await hotelAPI.getAllHotels();
    }
}
</script>

<style>

</style>