


let index = {
    init: function () {
        $("#btn-facebook").on("click", () => { // function(){} 대신에 ()=>{} 쓰는 이유는 this를 바인딩하기 위해서!
            this.facebook();
        });
        $("#btn-kakao").on("click", () => { // function(){} 대신에 ()=>{} 쓰는 이유는 this를 바인딩하기 위해서!
            this.kakao();
        });
        $("#btn-naver").on("click", () => { // function(){} 대신에 ()=>{} 쓰는 이유는 this를 바인딩하기 위해서!
            this.naver();
        });
        $("#btn-google").on("click", () => { // function(){} 대신에 ()=>{} 쓰는 이유는 this를 바인딩하기 위해서!
            this.google();
        });
    },

    facebook: function () {
        alert("test");
        return;
    },
    kakao: function () {

    },
    naver: function () {

    },
    google: function () {

    }
}

index.init();