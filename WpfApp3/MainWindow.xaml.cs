using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}
        //    private ModelVisual3D modelVisual3D;
        //    private TranslateTransform3D modelTransform;

        //    public MainWindow()
        //    {
        //        InitializeComponent();
        //        modelTransform = new TranslateTransform3D();
        //        viewport.Camera.Transform = modelTransform;

        //        try
        //        {
        //            modelVisual3D = LoadModel();
        //            viewport.Children.Add(modelVisual3D);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Failed to load model: " + ex.Message);
        //        }
        //    }

        //    private ModelVisual3D LoadModel()
        //    {
        //        Model3DGroup modelGroup = new Model3DGroup();

        //        OpenFileDialog openFileDialog = new OpenFileDialog();
        //        openFileDialog.Filter = "3D Models (*.obj, *.stl)|*.obj;*.stl";
        //        var modelImporter = new ModelImporter();

        //        var modelVisual3D = new ModelVisual3D();

        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            var model3D = modelImporter.Load(openFileDialog.FileName);

        //            // Добавление рельефа к модели
        //            var modelWithRelief = AddReliefToModel(model3D);

        //            modelVisual3D.Content = modelWithRelief;
        //        }
        //        return modelVisual3D;
        //    }

        //    private Model3D AddReliefToModel(Model3D model)
        //    {
        //        // Создание границы модели и ее смещения на противоположную сторону
        //        // Создание границы модели и ее смещения на противоположную сторону
        //        var bounds = model.Bounds;
        //        var center = bounds.Location;
        //        var inverseTranslation = new TranslateTransform3D(-center.X, -bounds.Y, -center.Z);

        //        // Создание рельефа для модели
        //        var reliefGeometry = new MeshGeometry3D();
        //        var d = model.Transform;
        //        Point3DCollection point3Ds = new Point3DCollection();
        //        point3Ds.Contains(center);
        //        reliefGeometry.Positions = point3Ds;

        //        // Установка высоты рельефа, чтобы модель не проваливалась
        //        double maxRelief = Math.Max(bounds.SizeX, Math.Max(bounds.SizeY, bounds.SizeZ));
        //        double reliefHeight = maxRelief * 0.1; // Высота рельефа (можно настроить)

        //        for (int i = 0; i < reliefGeometry.Positions.Count; i++)
        //        {
        //            var position = reliefGeometry.Positions[i];
        //            position.Y = reliefHeight * Math.Sin(position.X / maxRelief) * Math.Sin(position.Z / maxRelief);
        //            reliefGeometry.Positions[i] = position;
        //        }

        //        // Создание трансформаций модели с учетом рельефа
        //        var modelTransformGroup = new Transform3DGroup();
        //        modelTransformGroup.Children.Add(inverseTranslation);
        //        modelTransformGroup.Children.Add(model.Transform);
        //        //Material material = new Material();
        //        //material.InvalidateProperty(model.Transform.CloneCurrentValue().Inverse.DependencyObjectType.BaseType;
        //        //   var D = model.GetType(). as Material;
        //        // При создании рельефа, для материала используйте клонированный материал исходной модели
        //        var reliefModel = new GeometryModel3D(reliefGeometry,null);
        //        reliefModel.Transform = modelTransformGroup;

        //        return reliefModel;
        //    }

        //    private void Window_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        var delta = 10;

        //        if (e.Key == Key.W)
        //        {
        //            modelTransform.OffsetZ += delta;
        //        }

        //        if (e.Key == Key.S)
        //        {
        //            modelTransform.OffsetZ -= delta;
        //        }

        //        if (e.Key == Key.A)
        //        {
        //            modelTransform.OffsetX -= delta;
        //        }

        //        if (e.Key == Key.D)
        //        {
        //            modelTransform.OffsetX += delta;
        //        }
        //    }
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание геометрии куба
            var cubeMeshBuilder = new MeshBuilder();
            cubeMeshBuilder.AddBox(new Point3D(-3, -3, -3), 6, 6, 6); // Параметры: координаты центра и размеры куба

            // Создание геометрии круга
            var circleMeshBuilder = new MeshBuilder();
            circleMeshBuilder.AddCylinder(new Point3D(0, 0, -5), new Point3D(0, 0, 5), 4, 30); // Параметры: координаты начала и конца, радиус и количество сегментов

            // Создание моделей для куба и круга
            var cubeModel = new GeometryModel3D(cubeMeshBuilder.ToMesh(), Materials.Red);
            var circleModel = new GeometryModel3D(circleMeshBuilder.ToMesh(), Materials.Blue);

            // Создание контейнера для моделей
            var modelContainer = new Model3DGroup();
            modelContainer.Children.Add(cubeModel);
            modelContainer.Children.Add(circleModel);

            // Создание модели визуализации и добавление контейнера
            var modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = modelContainer;

            // Добавление модели в 3D-сцену
            viewport.Children.Add(modelVisual3D);
        }

        private TranslateTransform3D translation;

       public ModelVisual3D modelVisual3D { get; set; }
        //  private PerspectiveCamera camera;
        /// <summary>
        ///    private TranslateTransform3D translation;
        /// </summary>
        public Model3DGroup modelGroup;


        public double zoomFactor = 1.0;

     //   public OrthographicCamera camera;
        private PerspectiveCamera camera;

        private double cameraRotation = 0;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                var modelVisual3Da = LoadModel();
                var textureBrush = LoadTextureBrush();
                modelVisual3Da.Content = AddReliefToModel(modelVisual3Da.Content, textureBrush);
                modelVisual3D = modelVisual3Da;

                // Создание камеры
                camera = new PerspectiveCamera(new Point3D(0, 0, 200), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);               // Viewport3D.Camera = camera;

                // Инициализация объекта TranslateTransform3D
                translation = new TranslateTransform3D();

                modelVisual3D.Transform = translation;
                //((Transform3DGroup)modelVisual3D.Transform)
                    viewport.Children.Add(modelVisual3D);

                // Подписываемся на событие KeyDown окна
                KeyDown += Window_KeyDown;

                PreviewKeyDown += Window_PreviewKeyDown;

                //viewport.Children.Add(modelVisual3D);
            }
            catch
            {

            }
        }

        private ModelVisual3D LoadModel()
        {
            Model3DGroup modelGroup = new Model3DGroup();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3D Models (*.obj, *.stl)|*.obj;*.stl";
            var modelImporter = new ModelImporter();

            var modelVisual3D = new ModelVisual3D();

            if (openFileDialog.ShowDialog() == true)
            {
                var model3D = modelImporter.Load(openFileDialog.FileName);



                // Создание 3D-объекта
                modelVisual3D.Content = model3D;
            }
            return modelVisual3D;
        }

        private ImageBrush LoadTextureBrush()
        {
            var textureBrush = new ImageBrush();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                var bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                textureBrush.ImageSource = bitmapImage;
            }
            return textureBrush;
        }

        private Model3DGroup AddReliefToModel(Model3D model, ImageBrush textureBrush)
        {
            Model3DGroup modelGroup = new Model3DGroup();
            modelGroup.Children.Add(model);

            // Добавьте ваш ранее предоставленный код для обработки рельефа модели
            // ...

            return modelGroup;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var moveSpeed = 10;
                var rotationSpeed = 5;
                double delta = 0.1;
                if (e.Key == Key.W)
                {
                    translation.OffsetY -= delta;
                    //        zoomFactor -= 0.1;
                    // Движение вперед
                    var lookDirection = camera.LookDirection;
                    lookDirection.Normalize();
                    camera.Position += lookDirection * moveSpeed;
                }

                if (e.Key == Key.S)
                {
                       translation.OffsetY += delta;

                    // Движение назад
                    var lookDirection = camera.LookDirection;
                    lookDirection.Normalize();
                    camera.Position -= lookDirection * moveSpeed;
                }

                if (e.Key == Key.A)
                {
                           translation.OffsetX += delta;

                    // Движение влево
                    var sideDirection = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
                    sideDirection.Normalize();
                    camera.Position -= sideDirection * moveSpeed;

                    //    // Поворот камеры влево

                    cameraRotation += rotationSpeed;
                    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                }

                if (e.Key == Key.D)
                {
                          translation.OffsetX -= delta;

                    // Движение вправо
                    var sideDirection = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
                    sideDirection.Normalize();
                    camera.Position += sideDirection * moveSpeed;

                    //    // Поворот камеры вправо
                    cameraRotation -= rotationSpeed;
                    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                }

                //if (e.Key == Key.Q)
                //{
                //    // Поворот камеры влево
                //    cameraRotation += rotationSpeed;
                //    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                //    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                //}

                //if (e.Key == Key.E)
                //{
                //    // Поворот камеры вправо
                //    cameraRotation -= rotationSpeed;
                //    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                //    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                //}

            }
            catch
            {

            }
            //try
            //{
            //    double delta = 0.1;

            //    if (e.Key == Key.W)
            //    {
            //        translation.OffsetY -= delta;
            //        zoomFactor -= 0.1;
            //        camera.Width = 300 * zoomFactor;
            //    }

            //    if (e.Key == Key.S)
            //    {
                  
            //        translation.OffsetY += delta;

            //    }

            //    if (e.Key == Key.A)
            //    {
            //        translation.OffsetX += delta;
            //    }
                
            //    if (e.Key == Key.D)
            //    {
            //        translation.OffsetX -= delta;
            //    }
            //}
            //catch
            //{

            //}
            //try
            //{
            //    var transform = viewport.Camera.Transform as TranslateTransform3D;
            //    var delta = 10;

            //    if (transform != null)
            //    {
            //        if (e.Key == Key.W)
            //        {
            //            transform.OffsetZ += delta;
            //        }

            //        if (e.Key == Key.S)
            //        {
            //            transform.OffsetZ -= delta;
            //        }

            //        if (e.Key == Key.A)
            //        {
            //            transform.OffsetX -= delta;
            //        }

            //        if (e.Key == Key.D)
            //        {
            //            transform.OffsetX += delta;
            //        }
            //    }
            //}
            //catch
            //{

            //}
        }




        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var moveSpeed = 10;
                var rotationSpeed = 5;

                if (e.Key == Key.W)
                {
                    // Движение вперед
                    var lookDirection = camera.LookDirection;
                    lookDirection.Normalize();
                    camera.Position += lookDirection * moveSpeed;
                }

                if (e.Key == Key.S)
                {
                    // Движение назад
                    var lookDirection = camera.LookDirection;
                    lookDirection.Normalize();
                    camera.Position -= lookDirection * moveSpeed;
                }

                if (e.Key == Key.A)
                {
                    // Движение влево
                    var sideDirection = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
                    sideDirection.Normalize();
                    camera.Position -= sideDirection * moveSpeed;
                }

                if (e.Key == Key.D)
                {
                    // Движение вправо
                    var sideDirection = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
                    sideDirection.Normalize();
                    camera.Position += sideDirection * moveSpeed;
                }

                if (e.Key == Key.Q)
                {
                    // Поворот камеры влево
                    cameraRotation += rotationSpeed;
                    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                }

                if (e.Key == Key.E)
                {
                    // Поворот камеры вправо
                    cameraRotation -= rotationSpeed;
                    var rotationTransform = new RotateTransform3D(new AxisAngleRotation3D(camera.UpDirection, cameraRotation));
                    camera.LookDirection = rotationTransform.Transform(camera.LookDirection);
                }
            }
            catch
            {

            }
        }

        //public MainWindow()
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        viewport.Children.Add(LoadModel());
        //    }
        //    catch
        //    {

        //    }
        //}

        //private ModelVisual3D LoadModel()
        //{
        //    Model3DGroup modelGroup = new Model3DGroup();

        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "3D Models (*.obj, *.stl)|*.obj;*.stl";
        //    var modelImporter = new ModelImporter();

        //    var modelVisual3D = new ModelVisual3D();

        //    if (openFileDialog.ShowDialog() == true)
        //    {



        //        //  string filePath = openFileDialog.FileName;


        //        var model3D = modelImporter.Load(openFileDialog.FileName);

        //        // Создание 3D-объекта
        //        modelVisual3D.Content = model3D;
        //    }
        //    return modelVisual3D;
        //}

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        var transform = viewport.Camera.Transform as TranslateTransform3D;
        //        var delta = 10;

        //        if (transform != null)
        //        {
        //            if (e.Key == Key.W)
        //            {
        //                transform.OffsetZ += delta;
        //            }

        //            if (e.Key == Key.S)
        //            {
        //                transform.OffsetZ -= delta;
        //            }

        //            if (e.Key == Key.A)
        //            {
        //                transform.OffsetX -= delta;
        //            }

        //            if (e.Key == Key.D)
        //            {
        //                transform.OffsetX += delta;
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void OpenButton_Click(object sender, RoutedEventArgs e)
        //{

        //}



        //private void LoadModel_Click(OpenFileDialog filePath)
        //{
        //        // Загрузка модели
        //        var modelImporter = new ModelImporter();
        //        var model3D = modelImporter.Load(filePath.FileName);

        //        // Создание 3D-объекта
        //        var modelVisual3D = new ModelVisual3D();
        //        modelVisual3D.Content = model3D;

        //        // Добавление 3D-объекта на сцену
        //        viewport.Children.Add(modelVisual3D);

        //}

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {


        //        var transform = viewport.Camera.Transform as TranslateTransform3D;
        //        var delta = 10;


        //        if (transform != null)
        //        {
        //            if (e.Key == Key.W)
        //            {
        //                transform.OffsetZ += delta;
        //            }

        //            if (e.Key == Key.S)
        //            {
        //                transform.OffsetZ -= delta;
        //            }

        //            if (e.Key == Key.A)
        //            {
        //                transform.OffsetX -= delta;
        //            }

        //            if (e.Key == Key.D)
        //            {
        //                transform.OffsetX += delta;
        //            }
        //        }
        //       // base.OnKeyDown(e);
        //    }
        //    catch
        //    {

        //    }
        //}
    }
}


