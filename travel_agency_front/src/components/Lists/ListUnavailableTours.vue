<template>
    <div style="text-align: center;">
        <h1>Tours</h1>
        <div class="form-control">
            <input class="input input-alt" placeholder="Type name of tours" type="text" v-model="inputTour" @input="searchTour">
            <span class="input-border input-border-alt"></span>
        </div>
        <div class="error-search" v-if="searchedTours.length === 0">Incorrect tour name</div>


        <table class="list-table">
            <tr>
                <th>Image</th>
                <th>Name</th>
                <th>Start date</th>
                <th>End date</th>
                <th>Seats</th>
                <th>Price</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="tour in searchedTours" :key="tour.id">
                <td><img :src="tour.imageUrl"></td>
                <td>{{ tour.name }}</td>
                <td>{{ tour.startDate }}</td>
                <td>{{ tour.endDate }}</td>
                <td>{{ tour.quantitySeats }}</td>
                <td>{{ tour.price }}</td>
                <td>
                    <button style="margin-right: 30px;" class="button-update-delete button-update-delete-hover-green"
                        @click="enterUpdateTourPage(tour.id)">EDIT</button>
                    <button class="button-update-delete button-update-delete-hover-black" @click="deleteTour(tour.id)">DELETE</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as tourAPI from '@/services/API/tourAPI';
import * as dateHelper from '@/js/dateHelper';

export default {
    data() {
        return {
            tours: [],
            inputTour: "",
            searchedTours: []
        }
    },

    methods: {
        async searchTour() {
            this.searchedTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTour.toLowerCase()));
        },

        async enterUpdateTourPage(tourId) {
            this.$router.push(`/updateTour/${tourId}`);
        },

        async deleteTour(tourId) {
            console.log("Tour deleted! " + tourId);
        }
    },

    async mounted() {
        const token = localStorage.getItem('token');
        if (!token) {
            this.$router.push('/login');
            return;
        }
        this.tours = (await tourAPI.getUnavailableTours(token)).sort((a, b) => a.name.localeCompare(b.name));
        for (let tour of this.tours) {
            tour.startDate = await dateHelper.formatDate(tour.startDate);
            tour.endDate = await dateHelper.formatDate(tour.endDate);
        }
        this.searchedTours = this.tours;
    }
}
</script>

<style>
@import "./../../assets/css/styleTable.css";
@import "./../../assets/css/styleInputSearch.css";
</style>