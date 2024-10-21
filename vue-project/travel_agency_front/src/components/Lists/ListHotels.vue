<template>
    <div style="text-align: center;">
        <h1>Hotels</h1>
        <button class="button-create-action" @click="enterHotelCreatePage">Add hotel</button>

        <div class="form-control">
            <input class="input input-alt" placeholder="Type name of hotels" type="text" v-model="inputHotel" @input="searchHotel">
            <span class="input-border input-border-alt"></span>
        </div>
        <div class="error-search" v-if="searchedHotels.length === 0">Incorrect hotel name</div>


        <table class="list-table">
            <tr>
                <th>Image</th>
                <th>Hotel name</th>
                <th>Location</th>
                <th>Country</th>
                <th>Address</th>
                <th>Price per night</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="hotel in searchedHotels" :key="hotel.id">
                <td><img :src="hotel.imageUrl"></td>
                <td>{{ hotel.name }}</td>
                <td>{{ hotel.place.name }}</td>
                <td>{{ hotel.place.country }}</td>
                <td>{{ hotel.address }}</td>
                <td>{{ hotel.pricePerNight }}</td>
                <td>
                    <button style="margin-right: 30px;" class="button-update-delete button-update-delete-hover-green"
                        @click="enterUpdateHotelPage(hotel.id)">EDIT</button>
                    <button v-if="!usedHotelIds.includes(hotel.id)"
                        class="button-update-delete button-update-delete-hover-black"
                        @click="deleteHotelPage(hotel.id)">DELETE</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as hotelAPI from '@/services/API/hotelAPI';
import * as tourAPI from '@/services/API/tourAPI';

export default {
    data() {
        return {
            hotels: [],
            destinations: [],
            usedHotelIds: [],
            inputHotel: "",
            searchedHotels: []
        }
    },

    methods: {
        async searchHotel() {
            this.searchedHotels = this.hotels.filter(hotel => hotel.name.toLowerCase().includes(this.inputHotel.toLowerCase()));
        },

        async enterUpdateHotelPage(hotelId) {
            this.$router.push(`/updateHotel/${hotelId}`);
        },

        async deleteHotelPage(hotelId) {
            console.log("Hotel deleted! " + hotelId);
        },

        async enterHotelCreatePage() {
            this.$router.push('/createHotel');
        }

    },

    async mounted() {
        this.hotels = (await hotelAPI.getAllHotels()).sort((a, b) => a.place.name.localeCompare(b.place.name));
        this.searchedHotels = this.hotels;
        this.usedHotelIds = await tourAPI.getTourHotelsId();
    }
}
</script>

<style>
@import "./../../assets/css/styleTable.css";
@import "./../../assets/css/styleInputSearch.css";
@import "./../../assets/css/styleButtonCreate.css";
</style>