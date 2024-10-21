<template>
    <h1 style="margin-top: 30px;">Create Place</h1>
    <div class="create-form">
        <label for="name">Place Name:</label>
        <input id="name" type="text" v-model="name" placeholder="Enter place name" @input="checkCorrectInputs">
        <div class="error" v-if="!name">Name is required</div>

        <div class="create-form" v-for="(imageUrl, index) in imagesUrls" :key="index">
            <div>
                <label :for="'imageUrl' + index">Image URL:</label>
                <input :id="'imageUrl' + index" type="text" v-model="imagesUrls[index]" @input="validateImageUrl(index)"
                    required />
                <button style="margin-left: 30px;" type="button" @click="removeImage(index)">Remove</button>
            </div>

            <img v-if="existImages[index]" class="image-preview" :src="imageUrl" />
            <img v-else style="width: 300px;" class="image-preview" src='./@/../../../assets/images/image-not-found.jpg' />
        </div>
        <button v-if="!(imagesUrls.length >= 3)" type="button" @click="addImage">Add image</button>
        <div class="error" v-if="!isAllImages">Place must have 3 images</div>

        <label for="country">Country:</label>
        <input type="search" list="data" id="country" class="form-control" @input="checkCountryFromList" autocomplete="false"
            autocorrect="on" v-model="country" placeholder="Type to choose country">
        <datalist id="data">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </datalist>
        <div class="error" v-if="!isCountryFromList">Country is required</div>
        

        <label for="description">Description:</label>
        <textarea id="description" v-model="description" placeholder="Enter place description"
            @input="checkCorrectInputs"></textarea>
        <div class="error" v-if="!description">Description is required</div>

        <button v-if="!isSendRequest" style="margin: 20px;" @click="createPlace" :disabled="!isCorrectInputs">Add Place</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import { countries } from '@/js/countries';
import * as placeAPI from '@/services/API/placeAPI';

export default {
    data() {
        return {
            name: '',
            country: '',
            description: '',
            imagesUrls: [''],
            existImages: [false],
            isCorrectImage: false,
            countries: countries,
            isAllImages: false,
            isCorrectInputs: false,
            isCountryFromList: false,
            isSendRequest: false
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.name && this.isCountryFromList && this.description && this.isAllImages;
            console.log(this.isCorrectInputs);
        },

        async checkCountryFromList() {
            this.isCountryFromList = this.countries.includes(this.country);
            await this.checkCorrectInputs();
        },

        async clearFields() {
            this.name = '';
            this.country = '';
            this.description = '';
            this.imagesUrls = [''];
            this.existImages = [false];
        },

        async createPlace() {
            const data = {
                name: this.name,
                country: this.country,
                description: this.description,
                imagesUrls: this.imagesUrls
            }
            
            this.isSendRequest = true;
            const token = localStorage.getItem('token');
            if (!token) this.$router.push('/login');
            await placeAPI.createPlace(data, token);
            await this.clearFields();
            await this.checkCountryFromList();
            await this.validateImageUrl();
            this.isSendRequest = false;
        },

        async addImage() {
            if (this.imagesUrls.length === 5) {
                return;
            }
            this.imagesUrls.push('');
            this.existImages.push(false);
        },

        async removeImage(index) {
            this.imagesUrls.splice(index, 1);
            this.existImages.splice(index, 1);
        },

        checkImageExists(imageUrl) {
            return new Promise((resolve) => {
                let img = new Image();
                img.onload = () => resolve(true);
                img.onerror = () => resolve(false);
                img.src = imageUrl;
            });
        },

        async validateImageUrl(index) {
            this.existImages[index] = await this.checkImageExists(this.imagesUrls[index]);
            this.isAllImages = this.existImages.every((existImage) => existImage) && this.imagesUrls.length === 3;
            await this.checkCorrectInputs();
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleFormCreate.css";
</style>