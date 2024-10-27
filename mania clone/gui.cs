// WARNING : the following code you're about to inspect was made whilst i was bugging out at 2am
//         note to self: when you feel your soul escapinh from your body everytime you blink... you should probably go to sleep
















//using System.Numerics; // used for Vector2 struct, since it has SIMD implemented already and im to lazy to build SIMD optimised vector2 struct from scratch
// nevermind, it only nativly supprots floats, guess ill just implement SIMD afterall
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using Graphics;



namespace GUI
{
    public struct UIdimSIMD // ughhh ill do this later....
    { // ok fine ill do it now
    // ok but i should get a simpler working implementation first
        private Vector128<int> vector;
    }


    //fuck this
    //since when is UI going to ever be a bottlneck anyways

    //.. well i guess the bottleneck is the actual operating system since WriteConsole from kernel32 can only write so fast
    // which means i need every bit of performance posible to make up for it
    // but i can just run the rendering logic on another thread
    // so fine, ill just make a readable and easy to use UIdim struct
    /*
    public struct UIdim
    {
        public short pixelX;
        public short pixelY;
        public short percentX; // using fixed point behind the scenes since its 
        public short percentY; // much faster than floating point calculations


        public UIdim(short pixelX, short pixelY, float percentX, float percentY)
        {
            this.pixelX = pixelX;
            this.pixelY = pixelY;
            this.percentX = FloatToFixedPoint(percentX);
            this.percentY = FloatToFixedPoint(percentY);
        }
        public UIdim(short pixelX, short pixelY, short percentX, short percentY)
        {
            this.pixelX = pixelX;
            this.pixelY = pixelY;
            this.percentX = percentX;
            this.percentY = percentY;
        }

        private static short FloatToFixedPoint(float percent)
        {
            return (short)(percent * 65535);
        }

    }
    */

    // its already 2 am
    // every time i close my eyes i can feel my soul exiting my body
    public struct UIdim
    {
        public int pixelX;
        public int pixelY;
        public float percentX;
        public float percentY;


        // utility functions
        public int getAbsoluteX(int parentWidth)
        {
            return pixelX + (int)(parentWidth * percentX);
        }
        public int getAbsoluteY(int parentHeight)
        {
            return pixelY + (int)(parentHeight * percentY);
        }
        public UIdim getAbsolute(int parentWidth, int parentHeight)
        {
            return new UIdim(getAbsoluteX(parentWidth), getAbsoluteY(parentHeight),0f,0f);
        }

        // constructors
        public UIdim(int pixelX, int pixelY, float percentX, float percentY)
        {
            this.pixelX = pixelX;
            this.pixelY = pixelY;
            this.percentX = percentX;
            this.percentY = percentY;
        }

        public UIdim(int pixelX, float percentX, int pixelY, float percentY)
        {
            this.pixelX = pixelX;
            this.pixelY = pixelY;
            this.percentX = percentX;
            this.percentY = percentY;
        }





        // arithmatic operators
        public static UIdim operator +(UIdim a, UIdim b) => new UIdim(a.pixelX + b.pixelX, a.pixelY + b.pixelY, a.percentX + b.percentX, a.percentY + b.percentY);
        public static UIdim operator -(UIdim a, UIdim b) => new UIdim(a.pixelX - b.pixelX, a.pixelY - b.pixelY, a.percentX - b.percentX, a.percentY - b.percentY);
        public static UIdim operator *(UIdim a, UIdim b) => new UIdim(a.pixelX * b.pixelX, a.pixelY * b.pixelY, a.percentX * b.percentX, a.percentY * b.percentY);
        public static UIdim operator /(UIdim a, UIdim b) => new UIdim(a.pixelX / b.pixelX, a.pixelY / b.pixelY, a.percentX / b.percentX, a.percentY / b.percentY);
        public static UIdim operator *(float a, UIdim b) => new UIdim((int)(a * b.pixelX), (int)(a * b.pixelY), a * b.percentX, a * b.percentY);
        public static UIdim operator /(float a, UIdim b) => new UIdim((int)(a / b.pixelX), (int)(a / b.pixelY), a / b.percentX, a / b.percentY);
        public static UIdim operator *(UIdim b, float a) => new UIdim((int)(a * b.pixelX), (int)(a * b.pixelY), a * b.percentX, a * b.percentY);
        public static UIdim operator /(UIdim b, float a) => new UIdim((int)(a / b.pixelX), (int)(a / b.pixelY), a / b.percentX, a / b.percentY);
        public static UIdim operator *(int a, UIdim b) => new UIdim(a * b.pixelX, a * b.pixelY, a * b.percentX, a * b.percentY);
        public static UIdim operator /(int a, UIdim b) => new UIdim(a / b.pixelX, a / b.pixelY, a / b.percentX, a / b.percentY);
        public static UIdim operator *(UIdim b, int a) => new UIdim(a * b.pixelX, a * b.pixelY, a * b.percentX, a * b.percentY);
        public static UIdim operator /(UIdim b, int a) => new UIdim(a / b.pixelX, a / b.pixelY, a / b.percentX, a / b.percentY);
        public static UIdim operator -(UIdim a) => new UIdim(-a.pixelX, -a.pixelY, -a.percentX, -a.percentY);


        // comparrison operators
        public static bool operator ==(UIdim a, UIdim b) => a.pixelX == b.pixelX && a.pixelY == b.pixelX && a.percentX == b.percentX && a.percentY == b.percentY;
        public static bool operator !=(UIdim a, UIdim b) => !(a == b);


        // who even uses this
        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return this.pixelX;
                    case 1: return this.pixelX;
                    case 2: return this.pixelX;
                    case 3: return this.pixelX;
                    default: throw new ArgumentOutOfRangeException(nameof(index), index, "who the fuck even gets members of a struct by indexing it like a list\n its slower to use the square brackets you know ;)");
                }
            }
            set
            {
                switch (index)
                {
                    default: throw new Exception("no.. im not implementing that, i dont care if you like setting members of a struct using the square brakets. how about you try implementing it yourself at 2am.");
                }
            }
        }
    }

    public abstract class GuiModification
    {
        public abstract void Beffore(ref Window w, in GuiElement element, UIdim AbsolutePosition, UIdim AbsoluteSize);
        public abstract void After(ref Window w, in GuiElement element, UIdim AbsolutePosition, UIdim AbsoluteSize);
    }
    public abstract class GuiElement
    {
        public UIdim position;
        public UIdim size;
        public UIdim anchor;
        public List<GuiElement> children;
        public List<GuiModification> modifications;

        public void Append(GuiElement element)
        {
            children.Add(element);
        }
        public void Append(GuiModification element)
        {
            modifications.Add(element);
        }

        public abstract void Draw(ref Window w, UIdim AbsolutePosition, UIdim AbsoluteSize);
        private protected abstract void ApplyBefforeModifications(ref Window w, UIdim AbsolutePosition, UIdim AbsoluteSize);
        private protected abstract void ApplyAfterModifications(ref Window w, UIdim AbsolutePosition, UIdim AbsoluteSize);




        public GuiElement(UIdim position, UIdim size, UIdim anchor, List<GuiElement> children = null, List<GuiModification> modifications = null)
        {
            this.position = position;
            this.size = size;
            this.anchor = anchor;
            this.children = children;
            this.modifications = modifications;
        }
    }

    public class Frame : GuiElement
    {
        List<FrameModification> modifications;
        public Frame(UIdim position, UIdim size, UIdim anchor,List<GuiElement> children = null, List<FrameModification> modifications = null)
            : base(position, size, anchor, children) // implements default constructor behaviour
        {
            // extra constructor behaviour if needed
            this.modifications = modifications;
        }

        private protected override void ApplyBefforeModifications(ref Window w, UIdim TopLeftCorner, UIdim BottemRightCorner)
        {
            if (children == null) { return; }
            foreach (GuiModification modification in modifications)
            {
                modification.Beffore(ref w, this, TopLeftCorner, BottemRightCorner);
            }
        }
        private protected override void ApplyAfterModifications(ref Window w, UIdim TopLeftCorner, UIdim BottemRightCorner)
        {
            if (modifications == null) { return; }
            foreach (GuiModification modification in modifications)
            {
                modification.After(ref w, this, TopLeftCorner, BottemRightCorner);
            }
        }
        public override void Draw(ref Window w, UIdim AbsolutePosition, UIdim AbsoluteSize)
        {
            UIdim TopLeftCorner = AbsolutePosition - anchor;
            UIdim BottemRightCorner = TopLeftCorner + AbsoluteSize - new UIdim(1, 1, 0f, 0f);
            ApplyBefforeModifications(ref w, TopLeftCorner, BottemRightCorner);
        }
    }

    public abstract class FrameModification : GuiModification
    {
        public abstract override void Beffore(ref Window w, in GuiElement element, UIdim TopLeftCorner, UIdim BottemRightCorner);
    }

    public class Background : FrameModification
    {
        public override void Beffore(ref Window w, in GuiElement element, UIdim TopLeftCorner, UIdim BottemRightCorner)
        {
            throw new NotImplementedException();
        }
        public override void After(ref Window w, in GuiElement element, UIdim TopLeftCorner, UIdim BottemRightCorner)
        {
            throw new NotImplementedException();
        }
    }
}
