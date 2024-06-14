<template>
    <div style="text-align: center;" v-if="isLoaded">
        <h1>Update place</h1>

        <div class="create-form">
            <label for="name">Place Name:</label>
            <input id="name" type="text" v-model="place.name" placeholder="Enter place name" @input="checkCorrectInputs">
            <div class="error" v-if="!place.name">Name is required</div>

            <div class="create-form" v-for="(imageUrl, index) in place.imagesUrls" :key="index">
                <div>
                    <label :for="'imageUrl' + index">Image URL:</label>
                    <input :id="'imageUrl' + index" type="text" v-model="place.imagesUrls[index]"
                        @input="validateImageUrl(index)" required />
                    <button style="margin-left: 30px;" type="button" @click="removeImage(index)">Remove</button>
                </div>

                <img v-if="isCorrectImageUrl[index]" class="image-preview" :src="imageUrl" />
                <img v-else style="width: 300px;" class="image-preview"
                    src='./@/../../../assets/images/image-not-found.jpg' />
            </div>
            <button v-if="!(place.imagesUrls.length >= 3)" type="button" @click="addImage">Add image</button>
            <div class="error" v-if="place.imagesUrls.length != 3">Place must have 3 images</div>

            <label for="country">Country:</label>
            <input type="search" list="data" id="country" class="form-control"
                autocomplete="false" autocorrect="on" v-model="place.country" placeholder="Type to choose country">
            <datalist id="data">
                <option v-for="country in countries" :key="country" :value="country">
                    {{ country }}
                </option>
            </datalist>
            <div class="error" v-if="!countries.includes(place.country)">Country is required</div>


            <label for="description">Description:</label>
            <textarea id="description" v-model="place.description" placeholder="Enter place description"
                @input="checkCorrectInputs"></textarea>
            <div class="error" v-if="!place.description">Description is required</div>

            <button v-if="!isSendRequest" style="margin: 20px;" @click="updatePlace" :disabled="!isCorrectInputs">Update
                Place</button>
            <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button"
                disabled>
                <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
                <span role="status">Loading...</span>
            </button>
        </div>
    </div>
</template>

<script>
import { countries } from '@/js/countries';
import * as placeAPI from '@/services/API/placeAPI';

export default {
    data() {
        return {
            place: {
                name: '',
                country: '',
                description: '',
                imagesUrls: [],
            },
            startPlace: {},
            countries: countries,
            isCorrectImageUrl: [],
            isSendRequest: false,
            isCorrectInputs: false,
            isLoaded: false
        }
    },

    methods: {
        async checkCorrectInputs() {
            const isDifferent = JSON.stringify(this.place) !== JSON.stringify(this.startPlace);

            this.isCorrectInputs = this.place.name && this.place.description && this.isCorrectImageUrl.every((isCorrect) => isCorrect)
                && this.place.imagesUrls.length === 3 && this.countries.includes(this.place.country) && isDifferent;
        },


        async addImage() {
            if (this.place.imagesUrls.length === 3) {
                return;
            }
            this.place.imagesUrls.push('');
            this.isCorrectImage.push(false);
        },

        async removeImage(index) {
            this.place.imagesUrls.splice(index, 1);
            this.isCorrectImage.splice(index, 1);
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
            this.isCorrectImageUrl[index] = await this.checkImageExists(this.place.imagesUrls[index]);
            await this.checkCorrectInputs();
        },

        async updatePlace(){
            this.isSendRequest = true;
            await placeAPI.updatePlace(this.place.id, this.place);
            this.startPlace = JSON.parse(JSON.stringify(this.place));
            this.checkCorrectInputs();
            this.isSendRequest = false;        
        }
    },

    async mounted() {
        this.place = await placeAPI.getPlaceById(this.$route.params.id);
        this.place.imagesUrls = this.place.imagesUrls.map(imageUrl => imageUrl.url);
        this.startPlace = JSON.parse(JSON.stringify(this.place));
        for(let i = 0; i < this.place.imagesUrls.length; i++) {
            this.validateImageUrl(i);
        }
        this.isLoaded = true;
    }
}

</script>

<style>
@import "./../../assets/css/styleFormCreate.css";
</style>