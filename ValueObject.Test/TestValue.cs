namespace ValueObject.Test
{
    internal class TestValue : Cactus.Blade.ValueObject.ValueObject
    {
        public TestValue() { }

        public TestValue(int nonPublicValue)
        {
            ProtectedProperty = nonPublicValue;
            PrivateProperty = nonPublicValue;
            _privateField = nonPublicValue;
            ProtectedField = nonPublicValue;
        }

        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public int Field;
        protected int ProtectedField;
        private int _privateField;

        protected int ProtectedProperty { get; set; }
        private int PrivateProperty { get; set; }
    }
}