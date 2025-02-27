<script setup>
import { ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const formData = ref({
  username: "",
  password: "",
  passwordCheck: "",
});

const errorMessage = ref("");
const router = useRouter();

const submitRegistration = async () => {
  if (formData.value.password !== formData.value.passwordCheck) {
    errorMessage.value = "Passwords do not match!";
    return;
  }

  try {
    const response = await axios.post(
      "https://localhost:7245/api/users/signup",
      {
        username: formData.value.username,
        password: formData.value.password,
      },
      {
        headers: { "Content-Type": "application/json" },
      }
    );

    console.log("Registration successful:", response.data);

    alert("Registration successful! Welcome, " + formData.value.username);

    errorMessage.value = "";

    router.push("/login");
  } catch (error) {
    console.error("Error during registration:", error.response?.data?.message || error.message);
    errorMessage.value = error.response?.data?.message || "Registration error.";
  }
};
</script>





<template>
  <div class="form-container">
    <div class="form">
      <div class="pp-icon">
        <UnknownProfile></UnknownProfile>
      </div>
      <form @submit.prevent="submitRegistration">

        <div class="fields">
          <div class="field">
            <label for="username">Benutzername:</label>
            <input type="text" id="username" v-model="formData.username" required />
          </div>

          <div class="field">
            <label for="password">Passwort:</label>
            <input type="password" id="password" v-model="formData.password" required />
          </div>

          <div class="field">
            <label for="password_check">Passwort bestätigen:</label>
            <input type="password" id="password_check" v-model="formData.passwordCheck" required />
          </div>
        </div>

        <button class="btn-submit" type="submit">Registrieren</button>
        <button class="btn-login"><a href="login">Zum Login</a></button>
      </form>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script>

import axios from 'axios';

export default {
  data() {
    return {
      formData: {
        username: '',
        password: '',
        passwordCheck: ''
      }
    };
  },
  methods: {
    async submitRegistration() {
      console.log("Form data:", this.formData);
      if(this.formData.password !== this.formData.passwordCheck) {
        console.log("Passwort ungültig!");
        return;
      }

      const formData = new FormData();
      formData.append('name', this.formData.username);
      formData.append('password', this.formData.password);

      try {
        const response = await axios.post('http://localhost:7245/api/users/signup', this.formData);
        console.log('Formular erfolgreich gesendet:', response.data);
      } catch (error) {
        console.error('Fehler beim Senden des Formulars:', error);
      }
    }
  }
};
</script>

<style lang="scss" scoped>
@import '@/assets/css/theme';

.pp-icon {
  position: absolute;
  top: -40%;
  left: 50%;
  transform: translate(-50%, 40%);
  border-radius: 100%;
  box-shadow: inset -5px -5px 9px RGBA(1, 73, 124, 0.35), inset 5px 5px 9px RGBA(1, 73, 124, 0.4);
}

.form {
  margin: 0;
  position: relative;
  //position: absolute;
  //top: 50%;
  //left: 50%;
  //transform: translate(-50%, -50%);

  background: linear-gradient(145deg, #014270, #014e85);
  box-shadow:  25px 25px 37px #012b49,
  -25px -25px 37px #0167af;

  width: 540px;
  height: 640px; // 680px

  background: $layer_color_1;
  border-radius: 1.5rem;

  &-container {
    position: relative;
  }

  form {
    position: relative;
    display: flex;
    flex-direction: column;

    padding: 1rem;

  }
}

.fields {
  width: 80%;
  margin: 9rem auto 0;

  .field {
    display: flex;
    flex-direction: column;
    margin-bottom: 1rem;

    label {
      font-size: 1.25rem;
      margin: 0.25rem 0.5rem;
    }

    input {
      background: $input_fill;
      border: $input_border solid 1px;
      border-radius: 4px;
      min-height: 3rem;
      //box-shadow: inset 0.2rem 0.2rem 0.5rem #bec8e4, inset -0.2rem -0.2rem 0.5rem #fff;
      //box-shadow: inset 0 0 1rem #bec8e4, inset 0 0 10rem #fff;

      padding-left: 1rem;
      padding-right: 1rem;

      &:focus {
        outline: none;
        // #c8d0e7
        box-shadow: inset 0 0 1rem #c8d0e7, inset 0 0 1rem #bec8e4;
      }
    }

  }
}

.btn {
  &-submit {
    width: 70%;
    min-height: 3.5rem;

    margin: 1rem auto 0.5rem;
    background: $btn_primary;

    border: 0;
    border-radius: 4px;
    box-shadow: inset 2px 2px 4px #dd5c00,
    inset -2px -2px 4px #ff7600;

    font-size: 1.5rem;
    color: $white;
  }

  &-login {
    width: 50%;
    min-height: 1.75rem;
    margin: 1rem auto 0;
    border: 0;
    background: $btn_secondary;
    border-radius: 4px;
    box-shadow: inset 2px 2px 4px #dd5c00,
    inset -2px -2px 4px #ff7600;

    font-size: 1.125rem;
    color: $white;
  }
}

.error {
  color: $error_color;
}

a {
  color: $white;
  text-decoration: none;
}

</style>
