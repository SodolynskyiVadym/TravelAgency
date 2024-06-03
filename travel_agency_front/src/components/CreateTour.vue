<template>
    <h1>Create Tour</h1>
    <div class="create-form">
        <label for="name">Name:</label>
        <input id="name" type="text" v-model="name">

        <label for="description">Description:</label>
        <input id="description" type="text" v-model="description">

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="imageUrl">

        <div>
            <div v-for="(destination, index) in destinations" :key="index" class="create-form">
                <label :for="'country' + index">Country:</label>
                <input :id="'country' + index" type="search" list="countries" v-model="destinationsCountries[index]">
                <datalist id="countries">
                    <option v-for="country in countries" :key="country" :value="country">
                        {{ country }}
                    </option>
                </datalist>

                <label :for="'place' + index">Place:</label>
                <input :id="'place' + index" type="search" list="places" v-model="destinationsPlaces[index]"
                    placeholder="Type to choose place">
                <datalist id="places">
                    <option v-for="place in places" :key="place.name" :value="place.name">
                        {{ place.name }}
                    </option>
                </datalist>

                <label :for="'transport' + index">Transport:</label>
                <input :id="'transport' + index" type="search" list="transports" v-model="destinationsTransports[index]"
                    placeholder="Type to choose transport">
                <datalist id="transports">
                    <option v-for="transport in transports" :key="transport.name" :value="transport.name">
                        {{ transport.name }}
                    </option>
                </datalist>

                <label :for="'startDate' + index">Start Date:</label>
                <input :id="'startDate' + index" type="date" v-model="destination.startDate">

                <label :for="'endDate' + index">End Date:</label>
                <input :id="'endDate' + index" type="date" v-model="destination.endDate">
            </div>

            <button @click="addDestination">Add destination</button>
        </div>

        <label for="quantitySeats">Quantity of Seats:</label>
        <input id="quantitySeats" type="text" v-model="quantitySeats">

        <label for="price">Price:</label>
        <input id="price" type="number" v-model="price">
    </div>
</template>

<script>
// import * as tourAPI from '@/services/API/tourAPI';
import * as transportAPI from '@/services/API/transportAPI';
import * as hotelAPI from '@/services/API/hotelAPI';
import * as placeAPI from '@/services/API/placeAPI';
import { countries } from '@/js/countries';

export default {
    data() {
        return {
            name: '',
            description: '',
            destinations: [],
            hotels: [],
            transports: [],
            countries: countries,
            places: [],
            destinationsCountries: [],
            destinationsPlaces: [],
            destinationsTransports: []
        }
    },

    methods: {
        async addDestination() {
            this.destinations.push({
                startDate: '',
                endDate: ''
            });
        }

    },


    async mounted() {
        this.places = await placeAPI.getAllPlaces();
        this.transports = await transportAPI.getAllTransports();
        this.hotels = await hotelAPI.getAllHotels();
    }
}
</script>

<style>
@import "./../assets/css/styleFormCreate.css";
</style>