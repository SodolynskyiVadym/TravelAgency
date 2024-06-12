<template>
    <h1>Hotel's list</h1>
    <div>
        <table class="list-table">
            <tr>
                <th>Image</th>
                <th>Hotel name</th>
                <th>City</th>
                <th>Country</th>
                <th>Address</th>
                <th>Price per night</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="hotel in hotels" :key="hotel.id">
                <td><img :src="hotel.imageUrl"></td>
                <td>{{ hotel.name }}</td>
                <td>{{ hotel.place.name }}</td>
                <td>{{ hotel.place.country }}</td>
                <td>{{ hotel.address }}</td>
                <td>{{ hotel.pricePerNight }}</td>
                <td>
                    <button style="margin-right: 30px;" class="button-update-delete button-update-delete-hover-green" @click="enterUpdateHotelPage(hotel.id)">EDIT</button>
                    <button v-if="!usedHotelIds.includes(hotel.id)" class="button-update-delete button-update-delete-hover-black" @click="deleteHotelPage(hotel.id)">DELETE</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as hotelAPI from '@/services/API/hotelAPI';
import * as destinationAPI from '@/services/API/destinationAPI';

export default {
    data() {
        return {
            hotels: [],
            destinations: [],
            usedHotelIds: []
        }
    },

    methods: {
        async enterUpdateHotelPage(hotelId) {
            this.$router.push(`/updateHotel/${hotelId}`);
        },
        
        async deleteHotelPage(hotelId) {
            console.log("Hotel deleted! " + hotelId);
        },

    },

    async mounted() {
        this.hotels = (await hotelAPI.getAllHotels()).sort((a, b) => a.place.name.localeCompare(b.place.name));
        this.destinations = await destinationAPI.getAllDestinations();
        this.usedHotelIds = this.hotels.map(hotel => hotel.place.id);
    }
}
</script>

<style>
@import "./../../assets/css/styleTable.css";
</style>