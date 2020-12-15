import React, { Fragment } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Farm/Detail";
import { GET_FARM } from "../../utils/GraphQLQueries/queries";
import { useQuery } from "@apollo/client";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import { GET_PRODUCTS } from "../../utils/GraphQLQueries/queries";
import ErrorBlock from "../../components/atoms/ErrorBlock";

const FarmProductsBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;

  const { loading, data, error } = useQuery(GET_FARM, {
    variables: {
      criterias: {
        id: parseFloat(id),
      },
    },
  });

  const { productLoading, data: productData, error: productError } = useQuery(
    GET_PRODUCTS,
    {
      variables: {
        criterias: {
          farmId: parseFloat(id),
          page: 1,
          pageSize: 8,
        },
      },
    }
  );

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { farm: farmResponse } = data;
  let farm = { ...farmResponse };

  const breadcrumbs = [
    {
      title: "Farms",
      url: "/farms/",
    },
    {
      isActived: true,
      title: farm.name,
    },
  ];

  if (farm.thumbnails && farm.thumbnails.length > 0) {
    farm.images = farm.thumbnails.map((item) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
        image.url = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
      }
      return image;
    });
  }

  const renderProducts = (productLoading, productData, productError) => {
    if (productLoading || !productData) {
      return <Loading>Loading...</Loading>;
    } else if (productError) {
      return <ErrorBlock>Error!</ErrorBlock>;
    }

    const { products: ProductCollections } = productData;
    const { collections } = ProductCollections;

    const products = collections.map((item) => {
      let product = { ...item };
      product.url = `${UrlConstant.Product.url}${product.id}`;
      if (product.thumbnails && product.thumbnails.length > 0) {
        const thumbnail = product.thumbnails[0];
        if (thumbnail.pictureId > 0) {
          product.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.pictureId}`;
        }
      }

      product.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoCode) {
        product.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      if (product.productFarms) {
        product.productFarms = product.productFarms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.farmId}`;
          return productFarm;
        });
      }

      return product;
    });

    return (
      <div className="row">
        {products
          ? products.map((item, index) => {
              return (
                <div
                  key={index}
                  className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                >
                  <ProductItem product={item} />
                </div>
              );
            })
          : null}
      </div>
    );
  };

  return (
    <Fragment>
      <Detail farm={farm} breadcrumbs={breadcrumbs} />
      <FarmProductsBox>
        <TertiaryHeading>{farm.name}'s products</TertiaryHeading>
        {renderProducts(productLoading, productData, productError)}
      </FarmProductsBox>
    </Fragment>
  );
});
