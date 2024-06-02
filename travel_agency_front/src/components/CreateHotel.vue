<template>
    <div class="create-form">
        <label for="country">Country:</label>
        <input id="country" type="search" list="countries" class="form-control" @input="chooseCountry"
            autocomplete="false" autocorrect="on" v-model="country" placeholder="Type to choose country">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>

        <label for="placeName">Place Name:</label>
        <input id="placeName" type="search" list="data" v-model="placeName" @input="checkPlace"
            placeholder="Type to choose place">
        <datalist id="data">
            <option v-for="place in currentPlaces" :key="place.name" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>

        <label for="name">Hotel Name:</label>
        <input id="name" type="text" v-model="name" placeholder="Enter hotel name">

        <label for="address">Address:</label>
        <input id="address" type="text" v-model="address" placeholder="Enter place address">

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <img v-if="existImage" class="image-preview" :src="imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />

        <label for="pricePerNight">Price Per Night:</label>
        <input id="pricePerNight" type="number" v-model="pricePerNight" placeholder="Enter price per night">

        <label for="description">Description:</label>
        <textarea id="description" v-model="description" placeholder="Enter place description"></textarea>

        <button @click="createHotel">Create Hotel</button>
    </div>
</template>

<script>
import { countries } from '@/js/countries';
import * as placeAPI from '@/services/API/placeAPI';
import * as hotelAPI from '@/services/API/hotelAPI';

export default {
    data() {
        return {
            places: [],
            dataList: countries,
            isCorrectPlace: false,
            country: '',
            countries: countries,
            currentPlaces: [],
            name: '',
            placeName: '',
            placeId: 0,
            address: '',
            description: '',
            pricePerNight: 0,
            imageUrl: '',
            existImage: false
        }
    },

    methods: {
        checkImageExists(imageUrl) {
            return new Promise((resolve) => {
                let img = new Image();
                img.onload = () => resolve(true);
                img.onerror = () => resolve(false);
                img.src = imageUrl;
            });
        },

        async checkImage() {
            this.existImage = await this.checkImageExists(this.imageUrl);
        },

        async checkPlace() {
            if (this.places.map(place => place.name).includes(this.placeName)) {
                this.isCorrectPlace = true;
                this.country = this.places.find(place => place.name === this.placeName).country;
                this.placeId = this.places.find(place => place.name === this.placeName).id;
            }
            else {
                this.isCorrectPlace = false;
                this.chooseCountry();
                this.placeId = 0;
            }
        },

        async chooseCountry() {
            if (this.countries.includes(this.country)) {
                this.currentPlaces = this.places.filter(place => place.country === this.country);
            }
            else {
                this.placeName = '';
                this.currentPlaces = [];
            }
        },

        async clearFields() {
            this.name = '';
            this.placeName = '';
            this.country = '';
            this.placeId = 0;
            this.address = '';
            this.description = '';
            this.pricePerNight = 0;
        },

        async createHotel() {
            if (this.isCorrectPlace && this.address && this.description && this.pricePerNight > 0 && this.placeId > 0) {
                const data = {
                    name: this.name,
                    placeId: this.placeId,
                    address: this.address,
                    description: this.description,
                    imageUrl : this.imageUrl,
                    pricePerNight: this.pricePerNight
                }
                console.log(data);
                await hotelAPI.createHotel(data);
                await this.clearFields();
            }
        }
    },

    async mounted() {
        this.places = await placeAPI.getPlacesInfo();
        this.currentPlaces = this.places;
    }
}

</script>

<style>
@import "./../assets/css/styleFormCreate.css";
</style>