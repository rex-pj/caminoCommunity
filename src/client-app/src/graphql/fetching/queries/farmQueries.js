import { gql } from "@apollo/client";

export const GET_USER_FARMS = gql`
  query($criterias: FarmFilterModelInput) {
    userFarms(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoCode
        address
        pictures {
          pictureId
        }
      }
    }
  }
`;

export const GET_USER_FARMS_TITLE = gql`
  query($criterias: FarmFilterModelInput) {
    userFarms(criterias: $criterias) {
      collections {
        id
        name
      }
    }
  }
`;

export const GET_FARMS = gql`
  query($criterias: FarmFilterModelInput) {
    farms(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoCode
        address
        pictures {
          pictureId
        }
      }
    }
  }
`;

export const GET_FARM = gql`
  query($criterias: FarmFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoCode
      pictures {
        pictureId
      }
    }
  }
`;

export const GET_FARM_FOR_UPDATE = gql`
  query($criterias: FarmFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoCode
      farmTypeName
      farmTypeId
      pictures {
        pictureId
      }
    }
  }
`;
