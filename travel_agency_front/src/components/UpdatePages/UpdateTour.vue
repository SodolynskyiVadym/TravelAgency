<template>
    <h1>Create Tour</h1>
    <div class="create-form" v-if="isLoaded">
        <label for="name">Name:</label>
        <input id="name" type="text" v-model="tour.name" @input="checkCorrectInputs">
        <div class="error" v-if="!tour.name">Name is required</div>

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="tour.imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="tour.imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../../assets/images/image-not-found.jpg' />


        <label for="startDate">Start date</label>
        <input id="startDate" type="date" v-model="tour.startDate" @input="checkDestinationDates">
        <div class="error" v-if="new Date(tour.startDate) + 1 < new Date()">Start date can't be before today</div>
        <div class="error" v-if="new Date(tour.startDate) >= new Date(tour.endDate)">Start date can't be after or same
            end date
        </div>


        <label for="startPlaceCountry">Start location country</label>
        <input id="startPlaceCountry" list="countries" type="search" v-model="startPlaceCountry"
            placeholder="Enter start location country">
        <datalist id="countries">
            <option v-for="country in allCountries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!allCountries.includes(startPlaceCountry)">Country is required</div>

        <label for="startPlace">Start location</label>
        <input id="startPlace" list="places" type="search" v-model="startPlaceName" placeholder="Enter start location"
            @input="getStartPlaceId">
        <datalist id="places">
            <option v-for="place in allPlaces.filter(place => place.country === startPlaceCountry)" :key="place.name"
                :value="place.name">
                {{ place.name }}
            </option>
        </datalist>
        <div class="error" v-if="!allCountries.includes(startPlaceCountry)">Country is required for choice of place
        </div>
        <div class="error" v-if="tour.placeStartId === 0">Place is required</div>

        <div style="margin-top: 40px; margin-bottom: 40px">
            <div v-for="(destination, index) in tour.destinations" :key="index" class="create-form">
                <h2>Destination {{ index + 1 }}</h2>
                <label :for="'country' + index">Country:</label>
                <input :id="'country' + index" type="search" :list="'countries' + index"
                    @input="getDestinationPlaceIdByPlaceName(index)" v-model="destinationsCountries[index]"
                    placeholder="Type to choose country">
                <datalist :id="'countries' + index">
                    <option v-for="country in allCountries" :key="country" :value="country">
                        {{ country }}
                    </option>
                </datalist>
                <div class="error" v-if="!allCountries.includes(destinationsCountries[index])">Country is required</div>

                <label :for="'place' + index">Place:</label>
                <input :id="'place' + index" type="search" :list="'places' + index"
                    v-model="destinationsPlacesNames[index]" placeholder="Type to choose place"
                    @input="getDestinationPlaceIdByPlaceName(index)">
                <datalist :id="'places' + index">
                    <option v-for="place in allPlaces.filter(place => place.country === destinationsCountries[index])"
                        :key="place.name" :value="place.name">
                        {{ place.name }}
                    </option>
                </datalist>
                <div class="error" v-if="!allCountries.includes(destinationsCountries[index])">Country is required for
                    choice of place</div>
                <div class="error" v-if="!isCorrectPlacesNames[index]">This location don't exist or already used</div>

                <label :for="'hotel' + index">Hotel:</label>
                <input :id="'hotel' + index" type="search" :list="'hotels' + index"
                    v-model="destinationsHotelsNames[index]" placeholder="Type to choose hotel"
                    @input="getDestinationHotelIdByHotelName(index)">
                <datalist :id="'hotels' + index">
                    <option
                        v-for="hotel in allHotels.filter(hotel => hotel.placeId === tour.destinations[index].hotel.place.id)"
                        :key="hotel.name" :value="hotel.name">
                        {{ hotel.name }}
                    </option>
                </datalist>
                <div class="error" v-if="!isCorrectPlacesNames[index]">Place is required for choice of hotel</div>
                <div class="error" v-if="tour.destinations[index].hotelId === 0">Incorrect hotel</div>

                <label :for="'transport' + index">Transport:</label>
                <input :id="'transport' + index" type="search" list="transports"
                    v-model="destinationsTransportsNames[index]" placeholder="Type to choose transport"
                    @input="getDestinationTransportIdByTransportIdName(index)">
                <datalist id="transports">
                    <option v-for="transport in allTransports" :key="transport.type" :value="transport.name">
                        {{ transport.name }}
                    </option>
                </datalist>
                <div class="error" v-if="tour.destinations[index].transportId === 0">Transport is required</div>

                <label :for="'startDate' + index">Start Date:</label>
                <input :id="'startDate' + index" type="date" v-model="destination.startDate"
                    @input="checkDestinationDates">
                <div class="error" v-if="!isCorrectDestinationsStartDates[index]">Incorrect start date</div>

                <label :for="'endDate' + index">End Date:</label>
                <input :id="'endDate' + index" type="date" v-model="destination.endDate" @input="checkDestinationDates">
                <div class="error" v-if="!isCorrectDestinationsEndDates[index]">Incorrect end date</div>

                <button @click="removeDestination(index)">Remove destination</button>
            </div>


        </div>

        <button @click="addDestination">Add destination</button>
        
        <label for="endDate">End date</label>
        <input id="endDate" type="date" v-model="tour.endDate" @input="checkDestinationDates">

        <label for="endPlaceCountry">End location country</label>
        <input id="endPlaceCountry" list="endCountries" type="search" v-model="endPlaceCountry"
            placeholder="Enter end location country">
        <datalist id="endCountries">
            <option v-for="country in allCountries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!allCountries.includes(endPlaceCountry)">Country is required</div>

        <label for="endPlace">End location</label>
        <input id="endPlace" list="endPlaces" type="search" v-model="endPlaceName" placeholder="Enter end location"
            @input="getEndPlaceId">
        <datalist id="endPlaces">
            <option v-for="place in allPlaces.filter(place => place.country === endPlaceCountry)" :key="place.id"
                :value="place.name">
                {{ place.name }}
            </option>
        </datalist>
        <div class="error" v-if="!allCountries.includes(endPlaceCountry)">Country is required for choice of place</div>
        <div class="error" v-if="placeEndId === 0">Place is required</div>

        <label for="endPlaceTransport">End location transport</label>
        <input id="endPlaceTransport" type="search" list="transports" v-model="endTransportName"
            placeholder="Type to choose transport" @input="getEndPlaceTransport">
        <datalist id="transports">
            <option v-for="transport in transports" :key="transport.id" :value="transport.name">
                {{ transport.name }}
            </option>
        </datalist>
        <div class="error" v-if="tour.transportToEndId === 0">Transport is required</div>

        <label for="quantitySeats">Quantity of Seats(max {{ maxSeats }} seats):</label>
        <input id="quantitySeats" type="number" v-model="tour.quantitySeats" min="0" :max="maxSeats" @input="checkQuantitySeats">
        <div class="error" v-if="tour.quantitySeats <= 0 || tour.quantitySeats > maxSeats">Quantity of seats is required</div>

        <label for="description">Description:</label>
        <textarea id="description" v-model="tour.description" @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!tour.description">Description is required</div>

        <label for="price">Price:</label>
        <input id="price" type="number" v-model="tour.price" @input="checkCorrectInputs">
        <div class="error" v-if="tour.price <= 0">Price is required</div>


        <button v-if="!isSendRequest" style="margin: 20px;" @click="updateTour" :disabled="!isCorrectInputs">Update
            Tour</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>

        <label for="changeDate">Change date</label>
        <input id="changeDate" type="number" v-model="quantityDays">
        <button @click="changeTourDates">Change tour dates</button>
    </div>
</template>

<script>
import * as transportAPI from '@/services/API/transportAPI';
import * as hotelAPI from '@/services/API/hotelAPI';
import * as placeAPI from '@/services/API/placeAPI';
import { countries } from '@/js/countries';
import * as tourAPI from '@/services/API/tourAPI';

export default {
    data() {
        return {
            tour: null,
            allPlaces: [],
            allTransports: [],
            allHotels: [],
            allCountries: countries,
            startPlaceName: '',
            endPlaceName: '',
            startPlaceCountry: '',
            endPlaceCountry: '',
            endTransportName: '',
            destinationsCountries: [],
            destinationsPlacesNames: [],
            destinationsTransportsNames: [],
            destinationsHotelsNames: [],
            isCorrectDestinationsStartDates: [],
            isCorrectDestinationsEndDates: [],
            isCorrectPlacesNames: [],
            isCorrectInputs: false,
            isCorrectImageUrl: false,
            isSendRequest: false,
            isLoaded: false,
            quantityDays: 0,
            maxSeats: 0
        }
    },
    methods: {
        async checkCorrectInputs() {
            const hotelsIds = this.tour.destinations.map(destination => destination.hotelId);
            const transportsIds = this.tour.destinations.map(destination => destination.transportId);
            const isCorrectHotels = hotelsIds.filter(id => id != 0).length === this.tour.destinations.length;
            const isCorrectTransports = transportsIds.filter(id => id != 0).length === this.tour.destinations.length;


            
            this.isCorrectInputs = this.tour.name && this.isCorrectImageUrl && this.tour.description && this.tour.quantitySeats > 0 
            && this.tour.price > 0 && this.tour.placeStartId > 0 && this.tour.placeEndId > 0 && this.tour.transportToEndId > 0  && this.tour.quantitySeats <= this.maxSeats
            && this.tour.startDate < this.tour.endDate && this.tour.destinations.length > 0 && this.isCorrectDestinationsStartDates.every(date => date) && this.isCorrectDestinationsEndDates.every(date => date)
            && this.isCorrectPlacesNames.every(name => name) && isCorrectHotels && isCorrectTransports;
        },


        async calculateMaxSeats() {
            const tourTransports = this.allTransports.filter(transport => this.destinationsTransportsNames.includes(transport.name));
            if (tourTransports.length === 0) this.maxSeats = 0;
            else if (tourTransports.length === 1) this.maxSeats = tourTransports[0].quantitySeats;
            else this.maxSeats = Math.min(...tourTransports.map(transport => transport.quantitySeats));

            await this.checkCorrectInputs();
        },

        async findPlaceIdByPlaceName(placeName, countryName) {
            if (!this.allPlaces.find(place => place.name === placeName && place.country === countryName)) return 0;
            return this.allPlaces.find(place => place.name === placeName && place.country === countryName).id;
        },

        async findHotelIdByHotelName(hotelName, placeId) {
            if (!this.allHotels.find(hotel => hotel.name === hotelName && hotel.placeId === placeId)) return 0;
            return this.allHotels.find(hotel => hotel.name === hotelName && hotel.placeId === placeId).id;
        },

        async findTransportIdByTransportName(transportName) {
            if (!this.allTransports.find(transport => transport.name === transportName)) return 0;
            return this.allTransports.find(transport => transport.name === transportName).id;
        },

        async checkDestinationsPlacesNames() {
            const destinationsPlaceAndCountry = this.tour.destinations.map((destination, index) => [this.destinationsPlacesNames[index], this.destinationsCountries[index]]);
            for (let i = 0; i < destinationsPlaceAndCountry.length; i++) {
                this.isCorrectPlacesNames[i] = destinationsPlaceAndCountry.filter(arr => arr[0] === destinationsPlaceAndCountry[i][0]
                    && arr[1] === destinationsPlaceAndCountry[i][1]).length === 1 && this.allPlaces.find(place => place.name === destinationsPlaceAndCountry[i][0] && place.country === destinationsPlaceAndCountry[i][1]);
            }
        },

        async checkQuantitySeats(){
            if(this.tour.quantitySeats > this.maxSeats){
                this.tour.quantitySeats = this.maxSeats;
            }else if(this.tour.quantitySeats < 0){
                this.tour.quantitySeats = 0;
            }

            await this.checkCorrectInputs();
        },

        async getStartPlaceId() {
            this.tour.placeStartId = await this.findPlaceIdByPlaceName(this.startPlaceName, this.startPlaceCountry);
            await this.checkCorrectInputs();
        },

        async getEndPlaceId() {
            this.tour.placeEndId = await this.findPlaceIdByPlaceName(this.endPlaceName, this.endPlaceCountry);
            await this.checkCorrectInputs();
        },

        async getDestinationPlaceIdByPlaceName(index) {
            this.tour.destinations[index].hotel.place.id = await this.findPlaceIdByPlaceName(this.destinationsPlacesNames[index], this.destinationsCountries[index]);
            await this.checkDestinationsPlacesNames();
            await this.getDestinationHotelIdByHotelName(index);
            await this.checkCorrectInputs();
        },

        async getDestinationHotelIdByHotelName(index) {
            this.tour.destinations[index].hotelId = await this.findHotelIdByHotelName(this.destinationsHotelsNames[index], this.tour.destinations[index].hotel.place.id);
            await this.checkCorrectInputs();
        },

        async getDestinationTransportIdByTransportIdName(index) {
            this.tour.destinations[index].transportId = await this.findTransportIdByTransportName(this.destinationsTransportsNames[index]);
            await this.calculateMaxSeats();
            await this.checkCorrectInputs();
        },

        async getEndPlaceTransport() {
            this.tour.transportToEndId = await this.findTransportIdByTransportName(this.endTransportName);
            await this.calculateMaxSeats();
            await this.checkCorrectInputs();
        },

        async checkDestinationDates() {
            this.tour.destinations.forEach((destination, index) => {
                if (index === 0) {
                    this.isCorrectDestinationsStartDates[0] = new Date(destination.startDate) >= new Date(this.tour.startDate)
                        && new Date(destination.startDate) < new Date(destination.endDate);
                    this.isCorrectDestinationsEndDates[0] = new Date(destination.startDate) < new Date(destination.endDate);
                } else if (index === this.tour.destinations.length - 1) {
                    this.isCorrectDestinationsEndDates[this.tour.destinations.length - 1] = new Date(destination.endDate) <= new Date(this.tour.endDate)
                        && new Date(destination.startDate) < new Date(destination.endDate);
                    this.isCorrectDestinationsStartDates[this.tour.destinations.length - 1] = new Date(destination.startDate) < new Date(destination.endDate);
                } else {
                    this.isCorrectDestinationsStartDates[index] = new Date(destination.startDate) >= new Date(this.tour.destinations[index - 1].endDate)
                        && new Date(destination.startDate) < new Date(destination.endDate);
                    this.isCorrectDestinationsEndDates[index] = new Date(destination.endDate) <= new Date(this.tour.destinations[index + 1].startDate)
                        && new Date(destination.startDate) < new Date(destination.endDate);
                }
            });
            await this.checkCorrectInputs();
        },

        async addDestination() {
            this.tour.destinations.push({
                startDate: new Date().toISOString().split('T')[0],
                endDate: new Date().toISOString().split('T')[0],
                hotel: {
                    place: {
                        id: 0,
                    }
                },
            });

            this.destinationsCountries.push("");
            this.destinationsPlacesNames.push("");
            this.destinationsTransportsNames.push("");
            this.destinationsHotelsNames.push("");
            this.isCorrectDestinationsEndDates.push(false);
            this.isCorrectDestinationsStartDates.push(false);
            this.isCorrectPlacesNames.push(false);

            await this.checkDestinationDates();
            await this.checkCorrectInputs();
        },

        async removeDestination(index) {
            this.tour.destinations.splice(index, 1);

            this.destinationsCountries.splice(index, 1);
            this.destinationsPlacesNames.splice(index, 1);
            this.destinationsTransportsNames.splice(index, 1);
            this.destinationsHotelsNames.splice(index, 1);
            this.isCorrectDestinationsEndDates.splice(index, 1);
            this.isCorrectDestinationsStartDates.splice(index, 1);
            this.isCorrectPlacesNames.splice(index, 1);

            await this.checkDestinationDates();
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

        async changeTourDates(){
            this.quantityDays;
            for(let destination of this.tour.destinations){
                let dateStart = new Date(destination.startDate);
                dateStart.setDate(dateStart.getDate() + this.quantityDays);
                destination.startDate = dateStart.toISOString().split('T')[0];

                let dateEnd = new Date(destination.endDate);
                dateEnd.setDate(dateEnd.getDate() + this.quantityDays);
                destination.endDate = dateEnd.toISOString().split('T')[0];
            }

            let startDate = new Date(this.tour.startDate);
            startDate.setDate(startDate.getDate() + this.quantityDays);
            this.tour.startDate = startDate.toISOString().split('T')[0];

            let endDate = new Date(this.tour.endDate);
            endDate.setDate(endDate.getDate() + this.quantityDays);
            this.tour.endDate = endDate.toISOString().split('T')[0];

            await this.checkDestinationDates();
        },


        async updateTour() {
            this.isSendRequest = true;
            const token = localStorage.getItem('token');
            if (!token) this.$router.push('/login');
            await tourAPI.updateTour(this.tour, token);
            this.isSendRequest = false;
        }
    },

    async mounted() {
        this.allPlaces = await placeAPI.getAllPlaces();
        this.allTransports = await transportAPI.getAllTransports();
        this.allHotels = await hotelAPI.getAllHotels();
        this.tour = await tourAPI.getTourById(this.$route.params.id);

        this.tour.startDate = this.tour.startDate.split('T')[0];
        this.tour.endDate = this.tour.endDate.split('T')[0];
        this.startPlaceCountry = this.tour.placeStart.country;
        this.startPlaceName = this.tour.placeStart.name;
        this.endPlaceCountry = this.tour.placeEnd.country;
        this.endPlaceName = this.tour.placeEnd.name;
        this.endTransportName = this.tour.transportToEnd.name;

        this.tour.destinations = this.tour.destinations.sort((a, b) => new Date(a.startDate) - new Date(b.startDate));

        for (let destination of this.tour.destinations) {
            destination.startDate = destination.startDate.split('T')[0];
            destination.endDate = destination.endDate.split('T')[0];
            this.destinationsCountries.push(destination.hotel.place.country);
            this.destinationsPlacesNames.push(destination.hotel.place.name);
            this.destinationsTransportsNames.push(destination.transport.name);
            this.destinationsHotelsNames.push(destination.hotel.name);
            this.isCorrectDestinationsEndDates.push(false);
            this.isCorrectDestinationsStartDates.push(false);
            this.isCorrectPlacesNames.push(false);
        }
        await this.checkDestinationDates();
        await this.checkDestinationsPlacesNames();
        await this.checkImage();
        await this.calculateMaxSeats();
        await this.checkCorrectInputs();

        this.isLoaded = true;
    }
}
</script>


<style>
@import "./../../assets/css/styleFormCreate.css";
</style>