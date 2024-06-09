<template>
    <h1>Create Tour</h1>
    <div class="create-form">
        <label for="name">Name:</label>
        <input id="name" type="text" v-model="name" @input="checkCorrectInputs">
        <div class="error" v-if="!name">Name is required</div>

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />

        <label for="startDate">Start date</label>
        <input id="startDate" type="date" v-model="startDate" @input="checkCorrectInputs">
        <div class="error" v-if="new Date(startDate) + 1 < new Date()">Start date can't be before today</div>
        <div class="error" v-if="new Date(startDate) >= new Date(endDate)">Start date can't be after end date</div>

        <label for="startPlaceCountry">Start location country</label>
        <input id="startPlaceCountry" list="countries" type="search" v-model="startPlaceCountry"
            placeholder="Enter start location country" @input="checkIsCountryFromListStartLocation">
        <datalist id="countries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!countries.includes(startPlaceCountry)">Country is required</div>

        <label for="startPlace">Start location</label>
        <input id="startPlace" list="places" type="search" v-model="startPlace" placeholder="Enter start location"
            @input="checkStartPlace">
        <datalist id="places">
            <option v-for="place in possiblePlacesByCountryStartLocation" :key="place.name" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>
        <div class="error" v-if="!countries.includes(startPlaceCountry)">Country is required for choice of place</div>
        <div class="error" v-if="placeStartId === 0">Place
            is required</div>

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
                <div class="error" v-if="!countries.includes(destinationsCountries[index])">Country is required</div>

                <label :for="'place' + index">Place:</label>
                <input :id="'place' + index" type="search" :list="'places' + index" v-model="destinationsPlaces[index]"
                    placeholder="Type to choose place" @input="checkDestinationPlace(index)">
                <datalist :id="'places' + index">
                    <option v-for="place in possibleDestinationsPlacesByCountry[index]" :key="place.name"
                        :value="place.name">
                        {{ place.name }}
                    </option>
                </datalist>
                <div class="error" v-if="!countries.includes(destinationsCountries[index])">Country is required for
                    choice of place</div>
                <div class="error" v-if="!destinationsIsCorrectNames[index]">This location don't exist or already used
                </div>

                <label :for="'hotel' + index">Hotel:</label>
                <input :id="'hotel' + index" type="search" :list="'hotels' + index" v-model="destinationsHotels[index]"
                    placeholder="Type to choose hotel" @input="checkDestinationHotel(index)">
                <datalist :id="'hotels' + index">
                    <option v-for="hotel in possibleDestinationsHotelsByPlaceAndCountry[index]" :key="hotel.name"
                        :value="hotel.name">
                        {{ hotel.name }}
                    </option>
                </datalist>
                <div class="error"
                    v-if="!possibleDestinationsPlacesByCountry[index].find(place => place.name === destinationsPlaces[index])">
                    Place is required for choice of hotel</div>
                <div class="error"
                    v-if="!possibleDestinationsHotelsByPlaceAndCountry[index].find(hotel => hotel.name === destinationsHotels[index])">
                    Hotel is required</div>

                <label :for="'transport' + index">Transport:</label>
                <input :id="'transport' + index" type="search" list="transports" v-model="destinationsTransports[index]"
                    placeholder="Type to choose transport" @input="checkDestinationTransport(index)">
                <datalist id="transports">
                    <option v-for="transport in transports" :key="transport.type" :value="transport.name">
                        {{ transport.name }}
                    </option>
                </datalist>
                <div class="error"
                    v-if="!transports.find(transport => transport.name === destinationsTransports[index])">Transport is
                    required</div>

                <label :for="'startDate' + index">Start Date:</label>
                <input :id="'startDate' + index" type="date" v-model="destination.startDate"
                    @input="checkDestinationDates">
                <div class="error" v-if="!destinationsIsCorrectStartDates[index]">Incorrect start date</div>

                <label :for="'endDate' + index">End Date:</label>
                <input :id="'endDate' + index" type="date" v-model="destination.endDate" @input="checkDestinationDates">
                <div class="error" v-if="!destinationsIsCorrectEndDates[index]">Incorrect end date</div>

                <button @click="removeDestination(index)">Remove destination</button>
            </div>

            <button @click="addDestination">Add destination</button>
        </div>

        <label for="endDate">End date</label>
        <input id="endDate" type="date" v-model="endDate" @input="checkCorrectInputs">

        <label for="endPlaceCountry">End location country</label>
        <input id="endPlaceCountry" list="endCountries" type="search" v-model="endPlaceCountry"
            placeholder="Enter end location country" @input="checkIsCountryFromListEndLocation">
        <datalist id="endCountries">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!countries.includes(endPlaceCountry)">Country is required</div>

        <label for="endPlace">End location</label>
        <input id="endPlace" list="endPlaces" type="search" v-model="endPlace" placeholder="Enter end location"
            @input="checkEndPlace">
        <datalist id="endPlaces">
            <option v-for="place in possiblePlacesByCountryEndLocation" :key="place.id" :value="place.name">
                {{ place.name }}
            </option>
        </datalist>
        <div class="error" v-if="!countries.includes(endPlaceCountry)">Country is required for choice of place</div>
        <div class="error" v-if="placeEndId === 0">Place is
            required</div>

        <label for="endPlaceTransport">End location transport</label>
        <input id="endPlaceTransport" type="search" list="transports" v-model="endPlaceTransport"
            placeholder="Type to choose transport" @input="checkEndPlaceTransport">
        <datalist id="transports">
            <option v-for="transport in transports" :key="transport.id" :value="transport.name">
                {{ transport.name }}
            </option>
        </datalist>
        <div class="error" v-if="!transports.find(transport => transport.name === endPlaceTransport)">Transport is
            required</div>

        <label for="quantitySeats">Quantity of Seats:</label>
        <input id="quantitySeats" type="number" v-model="quantitySeats" @input="checkCorrectInputs">
        <div class="error" v-if="quantitySeats <= 0">Quantity of seats is required</div>

        <label for="description">Description:</label>
        <textarea id="description" v-model="description" @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!description">Description is required</div>

        <label for="price">Price:</label>
        <input id="price" type="number" v-model="price" @input="checkCorrectInputs">
        <div class="error" v-if="price <= 0">Price is required</div>


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
import * as transportAPI from '@/../services/API/transportAPI';
import * as hotelAPI from '@/../services/API/hotelAPI';
import * as placeAPI from '@/../services/API/placeAPI';
import * as tourAPI from '@/../services/API/tourAPI';
import { countries } from '@/../js/countries';

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
            isCorrectInputs: false,
            isSendRequest: false,
            placeStartId: 0,
            placeEndId: 0,
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
            destinationsIsCorrectStartDates: [],
            destinationsIsCorrectEndDates: [],
            destinationsIsCorrectNames: [],
            possibleDestinationsPlacesByCountry: [],
            possibleDestinationsHotelsByPlaceAndCountry: [],
            possiblePlacesByCountryStartLocation: [],
            possiblePlacesByCountryEndLocation: [],
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.name && this.isCorrectImageUrl && this.description && this.placeStartId != 0 && this.placeEndId != 0
                && this.quantitySeats > 0 && this.price > 0 && this.destinations.length > 0
                && this.destinations.every(destination => destination.placeId != 0 && destination.hotelId != 0 && destination.transportId != 0)
                && this.destinationsCountries.every(country => this.countries.includes(country))
                && this.destinationsIsCorrectEndDates.every(isCorrect => isCorrect)
                && this.destinationsIsCorrectStartDates.every(isCorrect => isCorrect) && new Date(this.startDate) >= new Date() && new Date(this.startDate) < new Date(this.endDate)
                && this.destinationsIsCorrectNames.every(isCorrect => isCorrect);
        },


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


        async checkDestinationDates() {
            this.destinationsIsCorrectEndDates = this.destinations.map((destination, i) => {

                if (i === this.destinations.length - 1) {
                    return new Date(destination.endDate) <= new Date(this.endDate) && new Date(destination.endDate) > new Date(destination.startDate);
                } else {
                    return new Date(destination.endDate) > new Date(destination.startDate);
                }
            });

            this.destinationsIsCorrectStartDates = this.destinations.map((destination, i) => {
                if (i === 0) {
                    return new Date(destination.startDate) >= new Date(this.startDate);
                } else {
                    return new Date(destination.startDate) >= new Date(this.destinations[i - 1].endDate) && new Date(destination.startDate) < new Date(destination.endDate);
                }
            });

            await this.checkCorrectInputs();
        },


        async checkIsCountryFromListStartLocation() {
            this.possiblePlacesByCountryStartLocation = this.places.filter(place => place.country === this.startPlaceCountry);
            this.placeStartId = await this.findPlaceIdByNameAndCountry(this.startPlace, this.startPlaceCountry);
            await this.checkCorrectInputs();
        },

        async checkIsCountryFromListEndLocation() {
            this.possiblePlacesByCountryEndLocation = this.places.filter(place => place.country === this.endPlaceCountry);
            this.placeEndId = await this.findPlaceIdByNameAndCountry(this.endPlace, this.endPlaceCountry);
            await this.checkCorrectInputs();
        },

        async checkEndPlaceTransport() {
            this.endPlaceTransportId = await this.findTransportIdByName(this.endPlaceTransport);
            await this.checkCorrectInputs();
        },

        async checkEndPlace() {
            this.placeEndId = await this.findPlaceIdByNameAndCountry(this.endPlace, this.endPlaceCountry);
            await this.checkDestinationsPlaceNames();
            await this.checkCorrectInputs();
        },

        async checkStartPlace() {
            this.placeStartId = await this.findPlaceIdByNameAndCountry(this.startPlace, this.startPlaceCountry);
            await this.checkDestinationsPlaceNames();
            await this.checkCorrectInputs();
        },


        async addDestination() {
            this.destinations.push({
                startDate: new Date().toISOString().split('T')[0],
                endDate: new Date().toISOString().split('T')[0]
            });
            this.destinationsCountries.push("");
            this.destinationsPlaces.push("");
            this.destinationsTransports.push("");
            this.destinationsHotels.push("");
            this.possibleDestinationsPlacesByCountry.push([]);
            this.possibleDestinationsHotelsByPlaceAndCountry.push([]);
            this.destinationsIsCorrectEndDates.push(false);
            this.destinationsIsCorrectStartDates.push(false);
            this.destinationsIsCorrectNames.push(false);
            await this.checkDestinationDates();
            await this.checkCorrectInputs();
        },

        async removeDestination(index) {
            this.destinations.splice(index, 1);
            this.destinationsCountries.splice(index, 1);
            this.destinationsPlaces.splice(index, 1);
            this.destinationsTransports.splice(index, 1);
            this.destinationsHotels.splice(index, 1);
            this.possibleDestinationsPlacesByCountry.splice(index, 1);
            this.possibleDestinationsHotelsByPlaceAndCountry.splice(index, 1);
            this.possiblePlacesByCountryStartLocation.splice(index, 1);
            this.possiblePlacesByCountryEndLocation.splice(index, 1);
            this.destinationsIsCorrectEndDates.splice(index, 1);
            this.destinationsIsCorrectStartDates.splice(index, 1);
            this.destinationsIsCorrectNames.splice(index, 1);
            await this.checkDestinationDates();
            await this.checkDestinationsPlaceNames();

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
            this.isCorrectImageUrl = await this.checkImageExists(this.imageUrl);
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
            } else {
                this.destinations[index].placeId = 0;
                this.destinationsHotels[index] = '';
                this.destinations[index].hotelId = 0;
            }
            await this.checkDestinationsPlaceNames();

            await this.checkCorrectInputs();
        },

        async checkDestinationTransport(index) {
            this.destinations[index].transportId = await this.findTransportIdByName(this.destinationsTransports[index]);
            await this.checkCorrectInputs();
        },

        async checkDestinationHotel(index) {
            this.destinations[index].hotelId = await this.findHotelIdByName(this.destinationsHotels[index]);
            await this.checkCorrectInputs();
        },

        async checkDestinationsPlaceNames() {
            const placesId = this.destinations.map((destination) => {
                return destination.placeId;
            });

            this.destinationsIsCorrectNames = this.destinations.map((destination, i) => {
                return this.places.map(place => place.name).includes(this.destinationsPlaces[i]) && placesId.filter(placeId => placeId === destination.placeId).length === 1
                    && this.placeStartId !== destination.placeId && this.placeEndId !== destination.placeId;
            });
        },


        async createTour() {
            this.isSendRequest = true;
            const data = {
                name: this.name,
                description: this.description,
                imageUrl: this.imageUrl,
                destinations: this.destinations,
                quantitySeats: this.quantitySeats,
                price: this.price,
                placeStartId: this.placeStartId,
                placeEndId: this.placeEndId,
                startDate: this.startDate,
                endDate: this.endDate,
                transportToEndId: this.endPlaceTransportId
            };
            await tourAPI.createTour(data);
            window.location.reload();
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
@import "./../../assets/css/styleFormCreate.css";
</style>