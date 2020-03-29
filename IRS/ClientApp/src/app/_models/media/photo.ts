export interface Photo {
    id: string;
    image: string; // represents src for image and video. in use by ng-image-slider
    thumbImage: string; // src
    title: string;
    alt: string;
}