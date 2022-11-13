import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const AuthService = class extends BaseService {
  login = async (request) => {
    return axios.post(apiConfig.paths.authentications.post.postLogin, request);
  };
};

export default AuthService;
