<template>
    <div style="text-align: center;">
        <h1>List of locations</h1>

        <div class="form-control">
            <input class="input input-alt" placeholder="Type name of location" type="text" v-model="inputPlace" @input="searchPlace">
            <span class="input-border input-border-alt"></span>
        </div>
        <div class="error-search" v-if="searchedPlaces.length === 0">Incorrect location name</div>

        <table class="list-table">
            <tr>
                <th>Name</th>
                <th>Country</th>
                <th>Image 1</th>
                <th>Image 2</th>
                <th>Image 3</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="place in searchedPlaces" :key="place.id">
                <td>{{ place.name }}</td>
                <td>{{ place.country }}</td>
                <td><img :src="place.imagesUrls[0].url"></td>
                <td><img :src="place.imagesUrls[1].url"></td>
                <td><img :src="place.imagesUrls[2].url"></td>
                <td>
                    <button style="margin-right: 30px;" class="button-update-delete button-update-delete-hover-green"
                        @click="enterUpdatePlacePage(place.id)">EDIT</button>
                    <button v-if="!(usedPlaceIds.includes(place.id) || usedPlaceStartIds.includes(place.id) || usedPlaceEndIds.includes(place.id))"
                        class="button-update-delete button-update-delete-hover-black"
                        @click="deletePlace(place.id)">DELETE</button>
                </td>
            </tr>
        </table>

    </div>
</template>

<script>
import * as placeAPI from '@/services/API/placeAPI';
import * as destinationAPI from '@/services/API/destinationAPI';
import * as tourAPI from '@/services/API/tourAPI';

export default {
    data() {
        return {
            places: [],
            inputPlace: "",
            searchedPlaces: [],
            usedPlaceIds: [],
            usedPlaceStartIds: [],
            usedPlaceEndIds : []
        }
    },

    methods: {
        async searchPlace() {
            this.searchedPlaces = this.places.filter(place => place.name.toLowerCase().includes(this.inputPlace.toLowerCase()));
        },

        async enterUpdatePlacePage(placeId) {
            console.log(`/updatePlace/${placeId}`)
            this.$router.push(`/updatePlace/${placeId}`);
        },

        async deletePlace(placeId) {
            console.log("Delete place with id: " + placeId);
        }
    },

    async mounted() {
        this.places = (await placeAPI.getAllPlaces()).sort((a, b) => a.country.localeCompare(b.country));
        const destinations = await destinationAPI.getAllDestinations();
        this.usedPlaceIds = destinations.map(destination => destination.hotel.placeId);

        const toursForeignKeys = await tourAPI.getAllToursForeignKeys();
        this.usedPlaceStartIds = toursForeignKeys.map(tour => tour.placeStartId);
        this.usedPlaceEndIds = toursForeignKeys.map(tour => tour.placeEndId);

        this.searchedPlaces = this.places;
    }
}
</script>

<style>
@import "./../../assets/css/styleTable.css";
@import "./../../assets/css/styleInputSearch.css";
</style>