<template>
    <h1>Create Transport</h1>
    <div class="create-form">
        <label for="name">Name</label>
        <input type="text" id="name" v-model="transport.name" @input="checkCorrectInputs">
        <div class="error" v-if="!transport.name">Name is required</div>

        <label for="typeTransp">Type of transport</label>
        <select id="typeTransp" v-model="transport.type" @input="checkCorrectInputs">
            <option value="Bus">Bus</option>
            <option value="Train">Train</option>
            <option value="Plane">Plane</option>
            <option value="Ship">Ship</option>
        </select>
        <div class="error" v-if="!transport.type">Type of transport is required</div>


        <label for="seats">Number of seats</label>
        <input type="number" id="seats" v-model="transport.quantitySeats" @input="checkCorrectInputs">
        <div class="error" v-if="transport.quantitySeats <= 0">Number of seats must be more than 0</div>

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="transport.imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="transport.imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../../assets/images/image-not-found.jpg' />


        <label for="description">Description</label>
        <textarea id="description" v-model="transport.description" @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!transport.description">Description is required</div>

        <button v-if="!isSendRequest" style="margin: 20px;" @click="updateTransport" :disabled="!isCorrectInputs">Update
            Transport</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import * as transportAPI from '@/services/API/transportAPI';

export default {
    data() {
        return {
            transport: {},
            isCorrectImageUrl: false,
            isSendRequest: false,
            isCorrectInputs: false,
            isLoaded: false,
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.transport.name && this.transport.description && this.transport.quantitySeats > 0 && this.isCorrectImageUrl;
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
            this.isCorrectImageUrl = await this.checkImageExists(this.transport.imageUrl);
            await this.checkCorrectInputs();
        },

        async updateTransport() {
            this.isSendRequest = true;
            this.isSendRequest = true;
            await transportAPI.updateTransport(this.transport.id, this.transport);
            this.isSendRequest = false;
        }
    },

    async mounted(){
        this.transport = await transportAPI.getTransportById(this.$route.params.id);
        this.isCorrectImageUrl = await this.checkImageExists(this.transport.imageUrl);
        this.isLoaded = true;
    }
}

</script>
<style>
@import "./../../assets/css/styleFormCreate.css";
</style>