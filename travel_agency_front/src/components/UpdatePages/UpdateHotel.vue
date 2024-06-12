<template>
    <div class="create-form">
        <label for="country">Country:</label>
        <input id="country" type="search" list="countries" class="form-control" @input="checkPlace" autocomplete="false"
            autocorrect="on" v-model="country" placeholder="Type to choose country">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!countries.includes(country)">Country is required</div>

        <label for="placeName">Place Name:</label>
        <input id="placeName" type="search" list="data" v-model="placeName" @input="checkPlace"
            placeholder="Type to choose place">
        <datalist id="data">
            <option v-for="place in places.filter(place => place.country === country)" :key="place.name"
                :value="place.name">
                {{ place.name }}
            </option>
        </datalist>
        <div class="error" v-if="tour.placeId === 0">Place is required</div>

        <label for="name">Hotel Name:</label>
        <input id="name" type="text" @input="checkCorrectInputs" v-model="tour.name" placeholder="Enter hotel name">
        <div class="error" v-if="!tour.name">Name is required</div>

        <label for="address">Address:</label>
        <input id="address" type="text" @input="checkCorrectInputs" v-model="tour.address" placeholder="Enter place address">
        <div class="error" v-if="!tour.address">Address is required</div>

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="tour.imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="tour.imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='@/assets/images/image-not-found.jpg' />

        <label for="pricePerNight">Price Per Night:</label>
        <input id="pricePerNight" type="number" v-model="tour.pricePerNight" min="0" @change="checkCorrectInputs"
            placeholder="Enter price per night" @input="checkPrice">
        <div class="error" v-if="tour.pricePerNight <= 0">Price must be more than o</div>

        <label for="description">Description:</label>
        <textarea id="description" v-model="tour.description" placeholder="Enter place description"
            @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!tour.description">Description is required</div>

        <button v-if="!isSendRequest" style="margin: 20px;" @click="createHotel" :disabled="!isCorrectInputs">Add
            Hotel</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import { countries } from '@/js/countries';
import * as placeAPI from '@/services/API/placeAPI';
import * as hotelAPI from '@/services/API/hotelAPI';

export default {
    data() {
        return {
            tour: {
                name: '',
                placeId: 0,
                address: '',
                description: '',
                pricePerNight: 0,
                imageUrl: '',
            },
            places: [],
            country: '',
            countries: countries,
            placeName: '',
            isCorrectImageUrl: false,
            isSendRequest: false,
            isCorrectInputs: false
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.tour.placeId != 0 && this.tour.address && this.tour.description && this.tour.pricePerNight > 0
                && this.isCorrectImageUrl && this.countries.includes(this.country);
        },

        async checkPrice() {
            if (this.tour.pricePerNight <= 0) {
                this.tour.pricePerNight = 0;
            }
            await this.checkCorrectInputs();
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
            this.isCorrectImageUrl = await this.checkImageExists(this.tour.imageUrl);
            await this.checkCorrectInputs();
        },

        async getPlaceIdByPlaceNameAndCountry(name, country) {
            if (this.places.find(place => place.name === name && place.country === country)) {
                return this.places.find(place => place.name === name && place.country === country).id;
            } else return 0;
        },

        async checkPlace() {
            this.tour.placeId = await this.getPlaceIdByPlaceNameAndCountry(this.placeName, this.country);
            await this.checkCorrectInputs();
        },

        async clearFields() {
            this.tour.name = '';
            this.placeName = '';
            this.country = '';
            this.tour.placeId = 0;
            this.tour.address = '';
            this.tour.description = '';
            this.tour.pricePerNight = 0;
            this.tour.imageUrl = '';
        },

        async createHotel() {
            if (this.isCorrectInputs) {
                this.isSendRequest = true;
                await hotelAPI.createHotel(this.tour);
                await this.clearFields();
                this.isCorrectInputs = false;
                this.isCorrectPlace = false;
                this.isCorrectImageUrl = false;
                this.isSendRequest = false;
            }
        }
    },

    async mounted() {
        this.places = await placeAPI.getPlacesInfo();
    }
}

</script>

<style>
@import "./../../assets/css/styleFormCreate.css";
</style>