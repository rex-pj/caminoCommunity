import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const MediaService = class extends BaseService {
  validatePicture = (request: { url?: string; file?: File }) => {
    return axios.put(apiConfig.paths.pictures.put.putValidatePicture, request);
  };
};

export default MediaService;
