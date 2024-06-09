<template>
    <h1>Create Transport</h1>
    <div class="create-form">
        <label for="name">Name</label>
        <input type="text" id="name" v-model="name" @input="checkCorrectInputs">
        <div class="error" v-if="!name">Name is required</div>

        <label for="typeTransp">Type of transport</label>
        <select id="typeTransp" v-model="typeTransport" @input="checkCorrectInputs">
            <option value="Bus">Bus</option>
            <option value="Train">Train</option>
            <option value="Plane">Plane</option>
            <option value="Ship">Ship</option>
        </select>
        <div class="error" v-if="!typeTransport">Type of transport is required</div>



        <label for="price">Price for hundred km</label>
        <input type="number" id="price" v-model="price" @input="checkCorrectInputs">
        <div class="error" v-if="price <= 0">Price must be more than 0</div>

        <label for="seats">Number of seats</label>
        <input type="number" id="seats" v-model="seats" @input="checkCorrectInputs">
        <div class="error" v-if="seats <= 0">Number of seats must be more than 0</div>

        <label for="imageUrl">Image URL:</label>
        <input id="imageUrl" type="text" v-model="imageUrl" placeholder="Enter image URL" @input="checkImage"
            required />
        <div class="error" v-if="!isCorrectImageUrl">Image url isn't correct</div>
        <img v-if="isCorrectImageUrl" class="image-preview" :src="imageUrl" />
        <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />


        <label for="description">Description</label>
        <textarea id="description" v-model="description" @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!description">Description is required</div>

        <button v-if="!isSendRequest" style="margin: 20px;" @click="createTransport" :disabled="!isCorrectInputs">Add Transport</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import * as transportAPI from '@/../services/API/transportAPI';

export default {
    data() {
        return {
            name: '',
            description: '',
            typeTransport: '',
            price: 0,
            seats: 0,
            imageUrl: '',
            isCorrectImageUrl: false,
            isSendRequest: false,
            isCorrectInputs: false
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.name && this.description && this.price > 0 && this.seats > 0 && this.isCorrectImageUrl;
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

        async clearFields() {
            this.name = '';
            this.description = '';
            this.typeTransport = '';
            this.price = 0;
            this.seats = 0;
            this.imageUrl = '';
            this.isCorrectImageUrl = false;
            this.isCorrectInputs = false;
        },

        async createTransport() {
            if(this.isCorrectInputs) {
                this.isSendRequest = true;
                const data = {
                    name: this.name,
                    description: this.description,
                    type: this.typeTransport,
                    quantitySeats: this.seats,
                    priceForHundredKm: this.price,
                    imageUrl: this.imageUrl
                };
                console.log(data)
                this.isSendRequest = true;
                await transportAPI.createTransport(data);
                await this.clearFields();
                this.isCorrectInputs = false;
                this.isSendRequest = false;

            }
        }
    }
}

</script>
<style>
@import "./../../assets/css/styleFormCreate.css";
</style>