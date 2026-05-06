ymaps.ready(init);

let coorKazan = [55.792386122295014, 49.12228410496998];
let coorMoscow = [55.75807687993509, 37.61323880859369];
let coorSpb = [59.9386, 30.3141];
let test = [55.79089602562555,49.12180918355625]; //кфу глав здание

// Конфигурация кастомных меток
const ICONS = {
    start: {
        href: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
        size: [40, 40],
        offset: [-20, -40]
    },
    end: {
        href: 'https://cdn-icons-png.flaticon.com/512/7945/7945007.png',
        size: [40, 40],
        offset: [-20, -40]
    }
};

// Стиль маршрута для лучшей видимости
const ROUTE_STYLE = {
    preset: 'islands#blueRoute',
    routeActiveStrokeColor: '#0066cc',      // Цвет активного участка
    routeActiveStrokeWidth: 6,               // Толщина активного участка
    routeInactiveStrokeColor: '#66b3ff',     // Цвет неактивного участка
    routeInactiveStrokeWidth: 4,             // Толщина неактивного участка
    routeStrokeColor: '#0066cc',             // Основной цвет
    routeStrokeWidth: 6,                     // Основная толщина
    routeStrokeOpacity: 0.9                  // Прозрачность
};

function init() {
    let map = new ymaps.Map('map', {
        center: coorKazan,
        zoom: 10,
        controls: []
    });

    // Кнопка "Очистить"
    let clearButton = new ymaps.control.Button({
        data: { content: "Очистить" },
        options: { selectOnClick: false }
    });
    clearButton.events.add('click', () => {
        map.geoObjects.removeAll();
    });

    // Удаляение ненужных контроллеров (явно, на случай если controls: [] не сработает)
    ['geolocationControl', 'searchControl', 'trafficControl', 
     'typeSelector', 'fullscreenControl', 'zoomControl', 'rulerControl']
        .forEach(name => map.controls.remove(name));

    // Добавляем кнопку очистки
    map.controls.add(clearButton, { float: 'left' });
}

// Функция добавления кастомной метки
function addCustomPlacemark(map, markerStyle, wayPoint) 
{
    let coordinates = wayPoint.geometry.getCoordinates();

    const placemark = new ymaps.Placemark(coordinates, {
        // hintContent: hintContent,
        // balloonContent: hintContent
    }, {
        iconLayout: 'default#image',
        iconImageHref: markerStyle.href,
        iconImageSize: markerStyle.size,
        iconImageOffset: markerStyle.offset
    });
    
    map.geoObjects.add(placemark);
    return placemark;
}

// Функция построения маршрута с кастомными метками
function buildCustomRoute(map, from, to) 
{
    ymaps.route([from, to], {
        // Настройки маршрута
        mapStateAutoApply: true,
        avoidTrafficJams: false
    }).then((route) => {
        // Применяем стиль для видимости
        route.options.set(ROUTE_STYLE);

        // Получаем все путевые точки маршрута (включая начальную, конечную и промежуточные)
        let wayPoints = route.getWayPoints();
        
        // Скрываем стандартные метки Яндекс.Маршрутизации
        wayPoints.each((wayPoint) => {
            wayPoint.options.set('visible', false);
        });

        addCustomPlacemark(map, ICONS.start, wayPoints.get(0))
        addCustomPlacemark(map, ICONS.end, wayPoints.get(wayPoints.getLength() - 1))

        // Добавляем маршрут на карту
        map.geoObjects.add(route);
        
        // Центрируем карту по маршруту
        map.setBounds(route.getBounds(), {
            checkZoomRange: true,
            zoomMargin: 50 // Отступ от краёв
        });
        
    }, (error) => {
        console.error('Ошибка построения маршрута:', error);
        alert('Не удалось построить маршрут. Попробуйте другой город.');
    });
}