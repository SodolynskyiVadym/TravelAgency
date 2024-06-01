<template>
    <h1 style="margin-top: 30px;">Create Place</h1>
    <div class="create-form">
        <input type="text" v-model="name" placeholder="Enter place name">

        <div class="create-form" v-for="(imageUrl, index) in imagesUrls" :key="index">
            <div>
                <input type="text" v-model="imagesUrls[index]" @input="validateImageUrl(index)" required />
                <button style="margin-left: 30px;" type="button" @click="removeImage(index)">Remove</button>
            </div>

            <img v-if="existImages[index]" class="image-preview" :src="imageUrl" />
            <img v-else style="width: 300px;" class="image-preview" src='./@/../../assets/images/image-not-found.jpg' />
        </div>

        <button type="button" @click="addImage">Add image</button>
        <!-- <div v-if="invalidActors">At least one actor is required</div> -->

        <select style="margin-top: 30px;" id="country" v-model="country">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </select><br>

        <textarea v-model="description" placeholder="Enter place description"></textarea>
        <button @click="createPlace">Create Place</button>
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
            countries: countries
        }
    },

    methods: {
        async createPlace() {
            const data = {
                name: this.name,
                country: this.country,
                description: this.description,
                imagesUrls: this.imagesUrls
            }

            await placeAPI.createPlace(data);
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
        }
    }
}
</script>

<style>
@import "./../assets/css/styleFormCreate.css";
</style>