<template>
    <h1>Create Tour</h1>
    <div class="create-form">
        <label for="name">Name:</label>
        <input id="name" type="text" v-model="name">

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />

        <label for="startDate">Start date</label>
        <input id="startDate" type="date" v-model="startDate">

        <label for="startPlaceCountry">Start location country</label>
        <input id="startPlaceCountry" list="countries" type="search" v-model="startPlaceCountry"
            placeholder="Enter start location country">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>

        <label for="startPlace">Start location</label>
        <input id="startPlace" list="places" type="search" v-model="startPlace" placeholder="Enter start location">
        <datalist id="places">
            <option v-for="place in places" :key="place.name" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>

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
                    <option v-for="transport in transports" :key="transport.type" :value="transport.name">
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

        <label for="endDate">Start date</label>
        <input id="endDate" type="date" v-model="endDate">

        <label for="endPlaceCountry">End location country</label>
        <input id="endPlaceCountry" list="countries" type="search" v-model="endPlaceCountry"
            placeholder="Enter end location country">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>

        <label for="endPlace">End location</label>
        <input id="endPlace" list="places" type="search" v-model="endPlace" placeholder="Enter end location">
        <datalist id="places">
            <option v-for="place in places" :key="place.id" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>

        <label for="endPlaceTransport">End location transport</label>
        <input id="endPlaceTransport" type="search" list="transports" v-model="endPlaceTransport"
            placeholder="Type to choose transport">
        <datalist id="transports">
            <option v-for="transport in transports" :key="transport.id" :value="transport.name">
                {{ transport.name }}
            </option>
        </datalist>

        <label for="quantitySeats">Quantity of Seats:</label>
        <input id="quantitySeats" type="text" v-model="quantitySeats">

        <label for="description">Description:</label>
        <textarea id="description" v-model="description"></textarea>

        <label for="price">Price:</label>
        <input id="price" type="number" v-model="price">


        <button v-if="!isSendRequest" style="margin: 20px;" @click="createTour" :disabled="!isCorrectInputs">Add
            Tour</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
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
            destinationsTransports: [],
            isCorrectInputs: false,
            isSendRequest: false,
            quantitySeats: 0,
            price: 0,
            startPlaceCountry: '',
            startPlace: '',
            imageUrl: '',
            isCorrectImageUrl: false,
            endPlaceCountry: '',
            endPlace: '',
            startDate: '',
            endDate: '',
        }
    },

    methods: {
        async addDestination() {
            this.destinations.push({
                startDate: '',
                endDate: ''
            });
        },


        async checkCorrectInputs() {
            this.isCorrectInputs = this.name && this.description && this.destinations.length > 0;
        },

        async checkImageExists(imageUrl) {
            return new Promise((resolve) => {
                let img = new Image();
                img.onload = () => resolve(true);
                img.onerror = () => resolve(false);
                img.src = imageUrl;
            });
        },

        async checkImage() {
            this.isCorrectImageUrl = await this.checkImageExists(this.imageUrl);
            await this.checkCorrectInputs();
        },

        async createTour() {
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