<template>
    <div style="text-align: center;">
        <h1>List of Transports</h1>

        <div class="form-control">
            <input class="input input-alt" placeholder="Type name of transport" type="text" v-model="inputTransport" @input="searchTransport">
            <span class="input-border input-border-alt"></span>
        </div>
        <div class="error" v-if="searchedTransports.length === 0">Incorrect transport name</div>


        <table class="list-table">
            <tr>
                <th>Image</th>
                <th>Name</th>
                <th>Type</th>
                <th>Price for hundred km</th>
                <th>Quantity of seats</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="transport in searchedTransports" :key="transport.id">
                <td><img :src="transport.imageUrl"></td>
                <td>{{ transport.name }}</td>
                <td>{{ transport.type }}</td>
                <td>{{ transport.priceForHundredKm }}</td>
                <td>{{ transport.quantitySeats }}</td>
                <td>
                    <button style="margin-right: 30px;" class="button-update-delete button-update-delete-hover-green"
                        @click="enterUpdateTransportPage(transport.id)">EDIT</button>
                    <button v-if="!usedTransportIds.includes(transport.id)"
                        class="button-update-delete button-update-delete-hover-black"
                        @click="deleteTransport(transport.id)">DELETE</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as transportAPI from '@/services/API/transportAPI';
import * as destinationAPI from '@/services/API/destinationAPI';

export default {
    data() {
        return {
            transports: [],
            destinations: [],
            usedTransportIds: [],
            inputTransport: "",
            searchedTransports: []
        }
    },

    methods: {
        async searchTransport() {
            this.searchedTransports = this.transports.filter(transport => transport.name.toLowerCase().includes(this.inputTransport.toLowerCase()));
        },

        async enterUpdateTransportPage(transportId) {
            this.$router.push(`/updateTransport/${transportId}`);
        },

        async deleteTransport(transportId) {
            console.log("Transport deleted! " + transportId);
        },

    },

    async mounted() {
        this.transports = await transportAPI.getAllTransports();
        this.searchedTransports = this.transports;
        this.destinations = await destinationAPI.getAllDestinations();
        this.usedTransportIds = this.destinations.map(destination => destination.transportId);
    }
}
</script>

<style>
@import "./../../assets/css/styleTable.css";
@import "./../../assets/css/styleInputSearch.css";
</style>