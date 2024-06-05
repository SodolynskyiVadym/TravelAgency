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
            placeholder="Enter start location country" @input="checkIsCountryFromListStartLocation">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>

        <label for="startPlace">Start location</label>
        <input id="startPlace" list="places" type="search" v-model="startPlace" placeholder="Enter start location">
        <datalist id="places">
            <option v-for="place in possiblePlacesByCountryStartLocation" :key="place.name" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>

        <div style="margin-top: 40px; margin-bottom: 40px">
            <div v-for="(destination, index) in destinations" :key="index" class="create-form">
                <h2>Destination {{ index + 1 }}</h2>
                <label :for="'country' + index">Country:</label>
                <input :id="'country' + index" type="search" :list="'countries' + index"
                    v-model="destinationsCountries[index]" @input="checkIsCountryFromListDestination(index)"
                    placeholder="Type to choose country">
                <datalist :id="'countries' + index">
                    <option v-for="country in countries" :key="country" :value="country">
                        {{ country }}
                    </option>
                </datalist>

                <label :for="'place' + index">Place:</label>
                <input :id="'place' + index" type="search" :list="'places' + index" v-model="destinationsPlaces[index]"
                    placeholder="Type to choose place" @input="checkDestinationPlace(index)">
                <datalist :id="'places' + index">
                    <option v-for="place in possibleDestinationsPlacesByCountry[index]" :key="place.name"
                        :value="place.name">
                        {{ place.name }}
                    </option>
                </datalist>

                <label :for="'hotel' + index">Hotel:</label>
                <input :id="'hotel' + index" type="search" :list="'hotels' + index" v-model="destinationsHotels[index]"
                    placeholder="Type to choose hotel" @input="checkDestinationHotel(index)">
                <datalist :id="'hotels' + index">
                    <option v-for="hotel in possibleDestinationsHotelsByPlaceAndCountry[index]" :key="hotel.name"
                        :value="hotel.name">
                        {{ hotel.name }}
                    </option>
                </datalist>

                <label :for="'transport' + index">Transport:</label>
                <input :id="'transport' + index" type="search" list="transports" v-model="destinationsTransports[index]"
                    placeholder="Type to choose transport" @input="checkDestinationTransport(index)">
                <datalist id="transports">
                    <option v-for="transport in transports" :key="transport.type" :value="transport.name">
                        {{ transport.name }}
                    </option>
                </datalist>

                <label :for="'startDate' + index">Start Date:</label>
                <input :id="'startDate' + index" type="date" v-model="destination.startDate">

                <label :for="'endDate' + index">End Date:</label>
                <input :id="'endDate' + index" type="date" v-model="destination.endDate">

                <button @click="removeDestination(index)">Remove destination</button>
            </div>

            <button @click="addDestination">Add destination</button>
        </div>

        <label for="endDate">Start date</label>
        <input id="endDate" type="date" v-model="endDate">

        <label for="endPlaceCountry">End location country</label>
        <input id="endPlaceCountry" list="endCountries" type="search" v-model="endPlaceCountry"
            placeholder="Enter end location country" @input="checkIsCountryFromListEndLocation">
        <datalist id="endCountries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>

        <label for="endPlace">End location</label>
        <input id="endPlace" list="endPlaces" type="search" v-model="endPlace" placeholder="Enter end location">
        <datalist id="endPlaces">
            <option v-for="place in possiblePlacesByCountryEndLocation" :key="place.id" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>

        <label for="endPlaceTransport">End location transport</label>
        <input id="endPlaceTransport" type="search" list="transports" v-model="endPlaceTransport"
            placeholder="Type to choose transport" @input="checkEndPlaceTransport">
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
            startPlaceCountry: '',
            startPlace: '',
            imageUrl: '',
            endPlaceCountry: '',
            endPlace: '',
            startDate: new Date().toISOString().split('T')[0],
            endDate: new Date().toISOString().split('T')[0],
            isCorrectImageUrl: false,
            isCorrectInputs: true,
            isSendRequest: false,
            startPlaceId: 0,
            endPlaceId: 0,
            endPlaceTransport: '',
            quantitySeats: 0,
            price: 0,
            destinations: [],
            hotels: [],
            transports: [],
            countries: countries,
            places: [],
            destinationsCountries: [],
            destinationsPlaces: [],
            destinationsTransports: [],
            destinationsHotels: [],
            possibleDestinationsPlacesByCountry: [],
            possibleDestinationsHotelsByPlaceAndCountry: [],
            possiblePlacesByCountryStartLocation: [],
            possiblePlacesByCountryEndLocation: [],
        }
    },

    methods: {
        async findPlaceIdByNameAndCountry(name, country) {
            if (this.places.find(place => place.name === name && place.country === country)) {
                return this.places.find(place => place.name === name && place.country === country).id;
            } else return 0;
        },

        async findTransportIdByName(name) {
            if (this.transports.find(transport => transport.name === name)) {
                return this.transports.find(transport => transport.name === name).id;
            } else return 0;
        },

        async findHotelIdByName(name) {
            if (this.hotels.find(hotel => hotel.name === name)) {
                return this.hotels.find(hotel => hotel.name === name).id;
            } else return 0;
        },


        async checkIsCountryFromListStartLocation() {
            this.possiblePlacesByCountryStartLocation = this.places.filter(place => place.country === this.startPlaceCountry);
            this.startPlaceId = await this.findPlaceIdByNameAndCountry(this.startPlace, this.startPlaceCountry);
            await this.checkCorrectInputs();
        },

        async checkIsCountryFromListEndLocation() {
            this.possiblePlacesByCountryEndLocation = this.places.filter(place => place.country === this.endPlaceCountry);
            this.endPlaceId = await this.findPlaceIdByNameAndCountry(this.endPlace, this.endPlaceCountry);
            await this.checkCorrectInputs();
        },


        async addDestination() {
            this.destinations.push({
                startDate: new Date().toISOString().split('T')[0],
                endDate: new Date().toISOString().split('T')[0]
            });
        },

        async removeDestination(index) {
            this.destinations.splice(index, 1);
            this.destinationsCountries.splice(index, 1);
            this.destinationsPlaces.splice(index, 1);
            this.destinationsTransports.splice(index, 1);
            this.possibleDestinationsPlacesByCountry.splice(index, 1);
        },


        async checkCorrectInputs() {
            // this.isCorrectInputs = this.name && this.description && this.destinations.length > 0;
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


        async checkEndPlaceTransport() {
            this.endPlaceTransportId = await this.findTransportIdByName(this.endPlaceTransport);
            await this.checkCorrectInputs();
        },

        async checkIsCountryFromListDestination(index) {
            if (this.countries.includes(this.destinationsCountries[index])) {
                this.possibleDestinationsPlacesByCountry[index] = this.places.filter(place => place.country === this.destinationsCountries[index]);
            } else {
                this.possibleDestinationsPlacesByCountry[index] = [];
                this.destinationsPlaces[index] = '';
                this.destinations[index].placeId = 0;
                this.destinationsHotels[index] = '';
                this.destinations[index].hotelId = 0;
            }
            await this.checkCorrectInputs();
        },

        async checkDestinationPlace(index) {
            if (this.places.find(place => place.name === this.destinationsPlaces[index] && place.country === this.destinationsCountries[index])) {
                this.destinations[index].placeId = await this.findPlaceIdByNameAndCountry(this.destinationsPlaces[index], this.destinationsCountries[index]);
                this.possibleDestinationsHotelsByPlaceAndCountry[index] = this.hotels.filter(hotel => hotel.placeId === this.destinations[index].placeId);
            }else{
                this.destinations[index].placeId = 0;
                this.destinationsHotels[index] = '';
                this.destinations[index].hotelId = 0;
            }
            await this.checkCorrectInputs();
        },

        async checkDestinationTransport(index) {
            this.destinations[index].transportsId = await this.findTransportIdByName(this.destinationsTransports[index]);
            await this.checkCorrectInputs();
        },

        async checkDestinationHotel(index) {
            this.destinations[index].hotelId = await this.findHotelIdByName(this.destinationsHotels[index]);
            await this.checkCorrectInputs();
        },


        async createTour() {
            const data = {
                name: this.name,
                description: this.description,
                imageUrl: this.imageUrl,
                destinations: this.destinations,
                quantitySeats: this.quantitySeats,
                price: this.price,
                startPlaceId: this.startPlaceId,
                endPlaceId: this.endPlaceId,
                startDate: this.startDate,
                endDate: this.endDate,
                transportToEndId: this.endPlaceTransportId
            };

            console.log(data);
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