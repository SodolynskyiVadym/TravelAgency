<template>
    <h1 style="margin-top: 30px;">Create Place</h1>
    <div class="create-form">
        <label for="name">Place Name:</label>
        <input id="name" type="text" v-model="name" placeholder="Enter place name">
        <div class="error" v-if="!name">Name is required</div>

        <div class="create-form" v-for="(imageUrl, index) in imagesUrls" :key="index">
            <div>
                <label :for="'imageUrl' + index">Image URL:</label>
                <input :id="'imageUrl' + index" type="text" v-model="imagesUrls[index]" @input="validateImageUrl(index)" required />
                <button style="margin-left: 30px;" type="button" @click="removeImage(index)">Remove</button>
            </div>

            <img v-if="existImages[index]" class="image-preview" :src="imageUrl" />
            <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />
        </div>
        <button v-if="!(imagesUrls.length >= 3)" type="button" @click="addImage">Add image</button>
        <div class="error" v-if="!isAllImagees">Place must have 3 images</div>

        <label for="country">Country:</label>
        <select id="country" v-model="country">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </select>
        <div class="error" v-if="!country">Country is required</div>

        <label for="description">Description:</label>
        <textarea id="description" v-model="description" placeholder="Enter place description"></textarea>
        <div class="error" v-if="!description">Description is required</div>

        <button style="margin-top: 20px;" @click="createPlace">Create Place</button>
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
            isAllImages: false
        }
    },

    methods: {
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

            await placeAPI.createPlace(data);
            await this.clearFields();
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
            this.isAllImagees = this.existImages.every((existImage) => existImage) && this.imagesUrls.length === 3;
        }
    }
}
</script>

<style>
@import "./../assets/css/styleFormCreate.css";
</style>