using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Gekko.Parser;
namespace Gekko
{
    public class TranslatedCode
    {
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static int labelCounter;
        public static readonly ScalarVal i3724 = new ScalarVal(1d);
        public static readonly ScalarVal i3726 = new ScalarVal(1d);
        public static readonly ScalarVal i3732 = new ScalarVal(1d);
        public static readonly ScalarVal i3736 = new ScalarVal(1d);
        public static readonly ScalarVal i3767 = new ScalarVal(1d);
        public static readonly ScalarVal i3771 = new ScalarVal(1d);
        public static readonly ScalarVal i3776 = new ScalarVal(1d);
        public static readonly ScalarVal i3778 = new ScalarVal(1d);
        public static readonly ScalarVal i3804 = new ScalarVal(1d);
        public static readonly ScalarVal i3808 = new ScalarVal(1d);
        public static readonly ScalarVal i3812 = new ScalarVal(1d);
        public static readonly ScalarVal i3816 = new ScalarVal(1d);
        public static readonly ScalarVal i3821 = new ScalarVal(1d);
        public static readonly ScalarVal i3823 = new ScalarVal(1d);
        public static readonly ScalarVal i3829 = new ScalarVal(1d);
        public static readonly ScalarVal i3833 = new ScalarVal(1d);
        public static readonly ScalarVal i3851 = new ScalarVal(1d);
        public static readonly ScalarVal i3853 = new ScalarVal(1d);
        public static readonly ScalarVal i3869 = new ScalarVal(1d);
        public static readonly ScalarVal i3871 = new ScalarVal(1d);
        public static readonly ScalarVal i3884 = new ScalarVal(1d);
        public static readonly ScalarVal i3885 = new ScalarVal(1d);
        public static readonly ScalarVal i3886 = new ScalarVal(1d);
        public static readonly ScalarVal i3888 = new ScalarVal(1d);
        public static readonly ScalarVal i3889 = new ScalarVal(1d);
        public static readonly ScalarVal i3890 = new ScalarVal(1d);
        public static readonly ScalarVal i3891 = new ScalarVal(1d);
        public static readonly ScalarVal i3893 = new ScalarVal(1d);
        public static readonly ScalarVal i3894 = new ScalarVal(1d);
        public static readonly ScalarVal i3895 = new ScalarVal(1d);
        public static readonly ScalarVal i3897 = new ScalarVal(1d);
        public static readonly ScalarVal i3898 = new ScalarVal(1d);
        public static readonly ScalarVal i3899 = new ScalarVal(1d);
        public static readonly ScalarVal i3900 = new ScalarVal(1d);
        public static readonly ScalarVal i3902 = new ScalarVal(1d);
        public static readonly ScalarVal i3903 = new ScalarVal(1d);
        public static readonly ScalarVal i3904 = new ScalarVal(1d);
        public static readonly ScalarVal i3906 = new ScalarVal(1d);
        public static readonly ScalarVal i3907 = new ScalarVal(1d);
        public static readonly ScalarVal i3908 = new ScalarVal(1d);
        public static readonly ScalarVal i3909 = new ScalarVal(1d);
        public static readonly ScalarVal i3911 = new ScalarVal(1d);
        public static readonly ScalarVal i3912 = new ScalarVal(1d);
        public static readonly ScalarVal i3913 = new ScalarVal(1d);
        public static readonly ScalarVal i3915 = new ScalarVal(1d);
        public static readonly ScalarVal i3916 = new ScalarVal(1d);
        public static readonly ScalarVal i3917 = new ScalarVal(1d);
        public static readonly ScalarVal i3918 = new ScalarVal(1d);
        public static readonly ScalarVal i3920 = new ScalarVal(1d);
        public static readonly ScalarVal i3921 = new ScalarVal(1d);
        public static readonly ScalarVal i3922 = new ScalarVal(1d);
        public static readonly ScalarVal i3924 = new ScalarVal(1d);
        public static readonly ScalarVal i3925 = new ScalarVal(1d);
        public static readonly ScalarVal i3926 = new ScalarVal(1d);
        public static readonly ScalarVal i3927 = new ScalarVal(1d);
        public static readonly ScalarVal i3930 = new ScalarVal(1d);
        public static readonly ScalarVal i3936 = new ScalarVal(1d);
        public static readonly ScalarVal i3954 = new ScalarVal(1d);
        public static readonly ScalarVal i3960 = new ScalarVal(1d);
        public static readonly ScalarVal i3978 = new ScalarVal(1d);
        public static readonly ScalarVal i3984 = new ScalarVal(1d);
        public static readonly ScalarVal i4002 = new ScalarVal(1d);
        public static readonly ScalarVal i4008 = new ScalarVal(1d);
        public static readonly ScalarVal i4026 = new ScalarVal(1d);
        public static readonly ScalarVal i4032 = new ScalarVal(1d);
        public static readonly ScalarVal i4050 = new ScalarVal(1d);
        public static readonly ScalarVal i4059 = new ScalarVal(1d);
        public static readonly ScalarVal i4060 = new ScalarVal(1d);
        public static readonly ScalarVal i4071 = new ScalarVal(1d);
        public static readonly ScalarVal i4072 = new ScalarVal(1d);
        public static readonly ScalarVal i4083 = new ScalarVal(1d);
        public static readonly ScalarVal i4084 = new ScalarVal(1d);
        public static readonly ScalarVal i4095 = new ScalarVal(1d);
        public static readonly ScalarVal i4096 = new ScalarVal(1d);
        public static readonly ScalarVal i4107 = new ScalarVal(1d);
        public static readonly ScalarVal i4108 = new ScalarVal(1d);
        public static readonly ScalarVal i4120 = new ScalarVal(1d);
        public static readonly ScalarVal i4121 = new ScalarVal(1d);
        public static readonly ScalarVal i4122 = new ScalarVal(1d);
        public static readonly ScalarVal i4133 = new ScalarVal(1d);
        public static readonly ScalarVal i4134 = new ScalarVal(1d);
        public static readonly ScalarVal i4135 = new ScalarVal(1d);
        public static readonly ScalarVal i4146 = new ScalarVal(1d);
        public static readonly ScalarVal i4147 = new ScalarVal(1d);
        public static readonly ScalarVal i4148 = new ScalarVal(1d);
        public static readonly ScalarVal i4159 = new ScalarVal(1d);
        public static readonly ScalarVal i4160 = new ScalarVal(1d);
        public static readonly ScalarVal i4161 = new ScalarVal(1d);
        public static readonly ScalarVal i4172 = new ScalarVal(1d);
        public static readonly ScalarVal i4173 = new ScalarVal(1d);
        public static readonly ScalarVal i4174 = new ScalarVal(1d);
        public static readonly ScalarVal i4186 = new ScalarVal(1d);
        public static readonly ScalarVal i4187 = new ScalarVal(1d);
        public static readonly ScalarVal i4188 = new ScalarVal(1d);
        public static readonly ScalarVal i4199 = new ScalarVal(1d);
        public static readonly ScalarVal i4200 = new ScalarVal(1d);
        public static readonly ScalarVal i4201 = new ScalarVal(1d);
        public static readonly ScalarVal i4212 = new ScalarVal(1d);
        public static readonly ScalarVal i4213 = new ScalarVal(1d);
        public static readonly ScalarVal i4214 = new ScalarVal(1d);
        public static readonly ScalarVal i4225 = new ScalarVal(1d);
        public static readonly ScalarVal i4226 = new ScalarVal(1d);
        public static readonly ScalarVal i4227 = new ScalarVal(1d);
        public static readonly ScalarVal i4238 = new ScalarVal(1d);
        public static readonly ScalarVal i4239 = new ScalarVal(1d);
        public static readonly ScalarVal i4240 = new ScalarVal(1d);
        public static readonly ScalarVal i4252 = new ScalarVal(1d);
        public static readonly ScalarVal i4253 = new ScalarVal(1d);
        public static readonly ScalarVal i4254 = new ScalarVal(1d);
        public static readonly ScalarVal i4260 = new ScalarVal(1d);
        public static readonly ScalarVal i4261 = new ScalarVal(1d);
        public static readonly ScalarVal i4262 = new ScalarVal(1d);
        public static readonly ScalarVal i4268 = new ScalarVal(1d);
        public static readonly ScalarVal i4269 = new ScalarVal(1d);
        public static readonly ScalarVal i4270 = new ScalarVal(1d);
        public static readonly ScalarVal i4281 = new ScalarVal(1d);
        public static readonly ScalarVal i4292 = new ScalarVal(1d);
        public static readonly ScalarVal i4303 = new ScalarVal(1d);
        public static readonly ScalarVal i4314 = new ScalarVal(1d);
        public static readonly ScalarVal i4325 = new ScalarVal(1d);
        public static readonly ScalarVal i4336 = new ScalarVal(1d);
        public static readonly ScalarVal i4337 = new ScalarVal(1d);
        public static readonly ScalarVal i4342 = new ScalarVal(2d);
        public static readonly ScalarVal i4343 = new ScalarVal(1d);
        public static readonly ScalarVal i4344 = new ScalarVal(1d);
        public static readonly ScalarVal i4345 = new ScalarVal(2d);
        public static readonly ScalarVal i4353 = new ScalarVal(1d);
        public static readonly ScalarVal i4354 = new ScalarVal(1d);
        public static readonly ScalarVal i4357 = new ScalarVal(1d);
        public static readonly ScalarVal i4358 = new ScalarVal(1d);
        public static readonly ScalarVal i4359 = new ScalarVal(1d);
        public static readonly ScalarVal i4361 = new ScalarVal(1d);
        public static readonly ScalarVal i4362 = new ScalarVal(1d);
        public static readonly ScalarVal i4364 = new ScalarVal(1d);
        public static readonly ScalarVal i4365 = new ScalarVal(1d);
        public static readonly ScalarVal i4366 = new ScalarVal(1d);
        public static readonly ScalarVal i4368 = new ScalarVal(1d);
        public static readonly ScalarVal i4369 = new ScalarVal(1d);
        public static readonly ScalarVal i4374 = new ScalarVal(1d);
        public static readonly ScalarVal i4375 = new ScalarVal(1d);
        public static readonly ScalarVal i4377 = new ScalarVal(1d);
        public static readonly ScalarVal i4379 = new ScalarVal(1d);
        public static readonly ScalarVal i4380 = new ScalarVal(1d);
        public static readonly ScalarVal i4381 = new ScalarVal(1d);
        public static readonly ScalarVal i4382 = new ScalarVal(1d);
        public static readonly ScalarVal i4384 = new ScalarVal(1d);
        public static readonly ScalarVal i4385 = new ScalarVal(1d);
        public static readonly ScalarVal i4386 = new ScalarVal(1d);
        public static readonly ScalarVal i4387 = new ScalarVal(1d);
        public static readonly ScalarVal i4388 = new ScalarVal(1d);
        public static readonly ScalarVal i4389 = new ScalarVal(1d);
        public static readonly ScalarVal i4390 = new ScalarVal(1d);
        public static readonly ScalarVal i4391 = new ScalarVal(1d);
        public static readonly ScalarVal i4392 = new ScalarVal(1d);
        public static readonly ScalarVal i4393 = new ScalarVal(1d);
        public static readonly ScalarVal i4394 = new ScalarVal(1d);
        public static readonly ScalarVal i4395 = new ScalarVal(1d);
        public static readonly ScalarVal i4396 = new ScalarVal(1d);
        public static readonly ScalarVal i4397 = new ScalarVal(1d);
        public static readonly ScalarVal i4398 = new ScalarVal(2d);
        public static readonly ScalarVal i4399 = new ScalarVal(1d);
        public static readonly ScalarVal i4400 = new ScalarVal(2d);
        public static readonly ScalarVal i4401 = new ScalarVal(2d);
        public static readonly ScalarVal i4408 = new ScalarVal(1d);
        public static readonly ScalarVal i4409 = new ScalarVal(1d);
        public static readonly ScalarVal i4410 = new ScalarVal(1d);
        public static readonly ScalarVal i4412 = new ScalarVal(1d);
        public static readonly ScalarVal i4413 = new ScalarVal(1d);
        public static readonly ScalarVal i4414 = new ScalarVal(1d);
        public static readonly ScalarVal i4415 = new ScalarVal(1d);
        public static readonly ScalarVal i4416 = new ScalarVal(1d);
        public static readonly ScalarVal i4417 = new ScalarVal(1d);
        public static readonly ScalarVal i4418 = new ScalarVal(1d);
        public static readonly ScalarVal i4420 = new ScalarVal(1d);
        public static readonly ScalarVal i4422 = new ScalarVal(1d);
        public static readonly ScalarVal i4423 = new ScalarVal(1d);
        public static readonly ScalarVal i4425 = new ScalarVal(1d);
        public static readonly ScalarVal i4428 = new ScalarVal(1d);
        public static readonly ScalarVal i4429 = new ScalarVal(1d);
        public static readonly ScalarVal i4434 = new ScalarVal(1d);
        public static readonly ScalarVal i4435 = new ScalarVal(1d);
        public static readonly ScalarVal i4437 = new ScalarVal(1d);
        public static readonly ScalarVal i4439 = new ScalarVal(1d);
        public static readonly ScalarVal i4440 = new ScalarVal(1d);
        public static readonly ScalarVal i4448 = new ScalarVal(1d);
        public static readonly ScalarVal i4450 = new ScalarVal(1d);
        public static readonly ScalarVal i4451 = new ScalarVal(1d);
        public static readonly ScalarVal i4476 = new ScalarVal(1d);
        public static readonly ScalarVal i4478 = new ScalarVal(1d);
        public static readonly ScalarVal i4481 = new ScalarVal(1d);
        public static readonly ScalarVal i4482 = new ScalarVal(1d);
        public static readonly ScalarVal i4483 = new ScalarVal(1d);
        public static readonly ScalarVal i4484 = new ScalarVal(1d);
        public static readonly ScalarVal i4486 = new ScalarVal(1d);
        public static readonly ScalarVal i4488 = new ScalarVal(1d);
        public static readonly ScalarVal i4495 = new ScalarVal(1d);
        public static readonly ScalarVal i4496 = new ScalarVal(1d);
        public static readonly ScalarVal i4498 = new ScalarVal(1d);
        public static readonly ScalarVal i4501 = new ScalarVal(1d);
        public static readonly ScalarVal i4507 = new ScalarVal(1d);
        public static readonly ScalarVal i4509 = new ScalarVal(1d);
        public static readonly ScalarVal i4513 = new ScalarVal(1d);
        public static readonly ScalarVal i4520 = new ScalarVal(1d);
        public static readonly ScalarVal i4521 = new ScalarVal(1d);
        public static readonly ScalarVal i4538 = new ScalarVal(1d);
        public static readonly ScalarVal i4540 = new ScalarVal(1d);
        public static readonly ScalarVal i4541 = new ScalarVal(1d);
        public static readonly ScalarVal i4542 = new ScalarVal(1d);
        public static readonly ScalarVal i4544 = new ScalarVal(1d);
        public static readonly ScalarVal i4545 = new ScalarVal(1d);
        public static readonly ScalarVal i4546 = new ScalarVal(1d);
        public static readonly ScalarVal i4548 = new ScalarVal(1d);
        public static readonly ScalarVal i4550 = new ScalarVal(1d);
        public static readonly ScalarVal i4552 = new ScalarVal(1d);
        public static readonly ScalarVal i4596 = new ScalarVal(1d);
        public static readonly ScalarVal i4599 = new ScalarVal(1d);
        public static readonly ScalarVal i4601 = new ScalarVal(1d);
        public static readonly ScalarVal i4602 = new ScalarVal(1d);
        public static readonly ScalarVal i4603 = new ScalarVal(1d);
        public static readonly ScalarVal i4612 = new ScalarVal(1d);
        public static readonly ScalarVal i4636 = new ScalarVal(1d);
        public static readonly ScalarVal i4638 = new ScalarVal(1d);
        public static readonly ScalarVal i4640 = new ScalarVal(0d);
        public static readonly ScalarVal i4643 = new ScalarVal(1d);
        public static readonly ScalarVal i4651 = new ScalarVal(1d);
        public static readonly ScalarVal i4652 = new ScalarVal(1d);
        public static readonly ScalarVal i4653 = new ScalarVal(1d);
        public static readonly ScalarVal i4654 = new ScalarVal(1d);
        public static readonly ScalarVal i4656 = new ScalarVal(1d);
        public static readonly ScalarVal i4657 = new ScalarVal(1d);
        public static readonly ScalarVal i4664 = new ScalarVal(1d);
        public static readonly ScalarVal i4665 = new ScalarVal(1d);
        public static readonly ScalarVal i4666 = new ScalarVal(1d);
        public static readonly ScalarVal i4667 = new ScalarVal(1d);
        public static readonly ScalarVal i4669 = new ScalarVal(0d);
        public static readonly ScalarVal i4670 = new ScalarVal(0d);
        public static readonly ScalarVal i4677 = new ScalarVal(1d);
        public static readonly ScalarVal i4678 = new ScalarVal(1d);
        public static readonly ScalarVal i4679 = new ScalarVal(1d);
        public static readonly ScalarVal i4681 = new ScalarVal(1d);
        public static readonly ScalarVal i4682 = new ScalarVal(1d);
        public static readonly ScalarVal i4683 = new ScalarVal(1d);
        public static readonly ScalarVal i4684 = new ScalarVal(1d);
        public static readonly ScalarVal i4685 = new ScalarVal(1d);
        public static readonly ScalarVal i4686 = new ScalarVal(1d);
        public static readonly ScalarVal i4688 = new ScalarVal(1d);
        public static readonly ScalarVal i4689 = new ScalarVal(1d);
        public static readonly ScalarVal i4690 = new ScalarVal(1d);
        public static readonly ScalarVal i4691 = new ScalarVal(1d);
        public static readonly ScalarVal i4706 = new ScalarVal(2d);
        public static readonly ScalarVal i4707 = new ScalarVal(2d);
        public static readonly ScalarVal i4708 = new ScalarVal(3d);
        public static readonly ScalarVal i4709 = new ScalarVal(3d);
        public static readonly ScalarVal i4711 = new ScalarVal(1d);
        public static readonly ScalarVal i4717 = new ScalarVal(1d);
        public static readonly ScalarVal i4719 = new ScalarVal(1d);
        public static readonly ScalarVal i4723 = new ScalarVal(1d);
        public static readonly ScalarVal i4726 = new ScalarVal(1d);
        public static readonly ScalarVal i4736 = new ScalarVal(1d);
        public static readonly ScalarVal i4749 = new ScalarVal(1d);
        public static readonly ScalarVal i4756 = new ScalarVal(1d);
        public static readonly ScalarVal i4769 = new ScalarVal(1d);
        public static readonly ScalarVal i4776 = new ScalarVal(1d);
        public static readonly ScalarVal i4798 = new ScalarVal(1d);
        public static readonly ScalarVal i4805 = new ScalarVal(1d);
        public static readonly ScalarVal d4814 = new ScalarVal(0.5d);
        public static readonly ScalarVal i4817 = new ScalarVal(1d);
        public static readonly ScalarVal i4819 = new ScalarVal(1d);
        public static readonly ScalarVal i4857 = new ScalarVal(1d);
        public static readonly ScalarVal i4864 = new ScalarVal(1d);
        public static readonly ScalarVal i4877 = new ScalarVal(1d);
        public static readonly ScalarVal i4884 = new ScalarVal(1d);
        public static readonly ScalarVal i4891 = new ScalarVal(1d);
        public static readonly ScalarVal i4898 = new ScalarVal(1d);
        public static readonly ScalarVal i4905 = new ScalarVal(1d);
        public static readonly ScalarVal i4918 = new ScalarVal(1d);
        public static readonly ScalarVal i4925 = new ScalarVal(1d);
        public static readonly ScalarVal i4932 = new ScalarVal(1d);
        public static readonly ScalarVal i4939 = new ScalarVal(1d);
        public static readonly ScalarVal i4946 = new ScalarVal(1d);
        public static readonly ScalarVal i5012 = new ScalarVal(1d);
        public static readonly ScalarVal i5016 = new ScalarVal(1d);
        public static readonly ScalarVal i5020 = new ScalarVal(1d);
        public static readonly ScalarVal i5024 = new ScalarVal(1d);
        public static readonly ScalarVal i5027 = new ScalarVal(1d);
        public static readonly ScalarVal i5028 = new ScalarVal(1d);
        public static readonly ScalarVal i5029 = new ScalarVal(1d);
        public static readonly ScalarVal i5030 = new ScalarVal(1d);
        public static readonly ScalarVal i5031 = new ScalarVal(1d);
        public static readonly ScalarVal i5032 = new ScalarVal(1d);
        public static readonly ScalarVal i5035 = new ScalarVal(1d);
        public static readonly ScalarVal i5036 = new ScalarVal(1d);
        public static readonly ScalarVal i5037 = new ScalarVal(1d);
        public static readonly ScalarVal i5040 = new ScalarVal(1d);
        public static readonly ScalarVal i5041 = new ScalarVal(1d);
        public static readonly ScalarVal i5042 = new ScalarVal(1d);
        public static readonly ScalarVal i5102 = new ScalarVal(1d);
        public static readonly ScalarVal i5106 = new ScalarVal(1d);
        public static readonly ScalarVal i5109 = new ScalarVal(1d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            foreach (IVariable listloop_x3415 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3721 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3415), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3415), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3415), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3415), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3415), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3415)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3721, o0, listloop_x3415)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            foreach (IVariable listloop_x3416 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o1)))
            {
                foreach (IVariable listloop_s3417 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o1)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3722 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3416, listloop_s3417), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3416, listloop_s3417), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3416, listloop_s3417), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3416, listloop_s3417), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3416, listloop_s3417), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3416, listloop_s3417)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pX_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3722, o1, listloop_x3416, listloop_s3417)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            foreach (IVariable listloop_x3418 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o2)))
            {
                foreach (IVariable listloop_s3419 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o2)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3723 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3418, listloop_s3419), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3418, listloop_s3419), O.Multiply(smpl, O.Add(smpl, i3724, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3418, listloop_s3419), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3418, listloop_s3419)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3419), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3419)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3723, o2, listloop_x3418, listloop_s3419)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            foreach (IVariable listloop_x3420 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o3)))
            {
                foreach (IVariable listloop_s3421 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o3)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3725 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3420, listloop_s3421), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3420, listloop_s3421), O.Multiply(smpl, O.Add(smpl, i3726, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3420, listloop_s3421), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3420, listloop_s3421)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3421), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3421)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3725, o3, listloop_x3420, listloop_s3421)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            foreach (IVariable listloop_x3422 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o4)))
            {
                foreach (IVariable listloop_s3423 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o4)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3727 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422, listloop_s3423), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422, listloop_s3423), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422, listloop_s3423), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422, listloop_s3423), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422, listloop_s3423), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422, listloop_s3423), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422)), O.Negate(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3422), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3422)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qX_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3727, o4, listloop_x3422, listloop_s3423)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            foreach (IVariable listloop_x3424 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o5)))
            {
                foreach (IVariable listloop_s3425 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o5)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3728 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425)), O.Negate(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3424, listloop_s3425), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3424, listloop_s3425)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3728, o5, listloop_x3424, listloop_s3425)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o6 = new O.Assignment();
            foreach (IVariable listloop_x3426 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o6)))
            {
                foreach (IVariable listloop_s3427 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o6)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3729 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427)), O.Negate(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3426, listloop_s3427), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3426, listloop_s3427)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3729, o6, listloop_x3426, listloop_s3427)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3734 = () =>
            {
                var smplCommandRemember3735 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3733 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3733.SetZero(smpl);

                foreach (IVariable listloop_x3731 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3733.InjectAdd(smpl, temp3733, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3732)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3731), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3731), O.Negate(smpl, i3732)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3731), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3731)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3735;
                return temp3733;

            };


            O.Assignment o7 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3730 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qX_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func3734(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3736)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i3736)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3730, o7, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o8 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3737 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pX_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3737, o8, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o9 = new O.Assignment();
            foreach (IVariable listloop_x3428 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o9)))
            {
                foreach (IVariable listloop_s3429 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o9)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3738 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3428, listloop_s3429), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3428, listloop_s3429), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3428, listloop_s3429), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3428, listloop_s3429)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3428, listloop_s3429), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3428, listloop_s3429));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3738, o9, listloop_x3428, listloop_s3429)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o10 = new O.Assignment();
            foreach (IVariable listloop_x3430 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o10)))
            {
                foreach (IVariable listloop_s3431 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o10)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3739 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3430, listloop_s3431), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3430, listloop_s3431), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3430, listloop_s3431), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3430, listloop_s3431), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3430, listloop_s3431), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3430, listloop_s3431)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3739, o10, listloop_x3430, listloop_s3431)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o11 = new O.Assignment();
            foreach (IVariable listloop_x3432 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o11)))
            {
                foreach (IVariable listloop_s3433 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o11)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3740 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3432, listloop_s3433), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3432, listloop_s3433), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3432, listloop_s3433), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3432, listloop_s3433), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3432, listloop_s3433), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3432, listloop_s3433)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3740, o11, listloop_x3432, listloop_s3433)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3744 = (IVariable listloop_x3434) =>
            {
                var smplCommandRemember3745 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3743 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3743.SetZero(smpl);

                foreach (IVariable listloop_s3742 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3743.InjectAdd(smpl, temp3743, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3434, listloop_s3742), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3434, listloop_s3742));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3745;
                return temp3743;

            };


            O.Assignment o12 = new O.Assignment();
            foreach (IVariable listloop_x3434 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o12)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3741 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3434), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3434), func3744(listloop_x3434));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3741, o12, listloop_x3434)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3749 = () =>
            {
                var smplCommandRemember3750 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3748 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3748.SetZero(smpl);

                foreach (IVariable listloop_c3747 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3748.InjectAdd(smpl, temp3748, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3747), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3747), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3747), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCTourist", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3747)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3750;
                return temp3748;

            };


            O.Assignment o13 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3746 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vX_XTOU", null, null, new LookupSettings(), EVariableType.Var, null), func3749());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3746, o13, new ScalarString("XTOU"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3754 = () =>
            {
                var smplCommandRemember3755 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3753 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3753.SetZero(smpl);

                foreach (IVariable listloop_x3752 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3753.InjectAdd(smpl, temp3753, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3752), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3752));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3755;
                return temp3753;

            };


            O.Assignment o14 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3751 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vX_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3754());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3751, o14, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3759 = (IVariable listloop_i3435) =>
            {
                var smplCommandRemember3760 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3758 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3758.SetZero(smpl);

                foreach (IVariable listloop_s3757 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3758.InjectAdd(smpl, temp3758, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3435, listloop_s3757), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3435, listloop_s3757));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3760;
                return temp3758;

            };


            O.Assignment o15 = new O.Assignment();
            foreach (IVariable listloop_i3435 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o15)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3756 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3435), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_i_tot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3435), func3759(listloop_i3435));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3756, o15, listloop_i3435, new ScalarString("tot"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o16 = new O.Assignment();
            foreach (IVariable listloop_i3436 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o16)))
            {
                foreach (IVariable listloop_s3437 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o16)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3761 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, listloop_s3437), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, listloop_s3437), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, listloop_s3437), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, listloop_s3437), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, listloop_s3437), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, listloop_s3437)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436))), O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, new ScalarString("tot")), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, new ScalarString("PUB")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3436, listloop_s3437), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3436, listloop_s3437)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3761, o16, listloop_i3436, listloop_s3437)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o17 = new O.Assignment();
            foreach (IVariable listloop_i3438 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o17)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3762 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3438), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_s_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3438), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3438, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3438, new ScalarString("PUB")), O.Lookup(smpl, null, null, "vPublicDirectInvestment", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3438, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3438, new ScalarString("PUB"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3762, o17, listloop_i3438, new ScalarString("PUB"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o18 = new O.Assignment();
            foreach (IVariable listloop_i3439 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o18)))
            {
                foreach (IVariable listloop_s3440 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o18)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3763 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3439, listloop_s3440), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3439, listloop_s3440)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3763, o18, listloop_i3439, listloop_s3440)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o19 = new O.Assignment();
            foreach (IVariable listloop_i3441 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o19)))
            {
                foreach (IVariable listloop_s3442 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o19)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3764 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3441, listloop_s3442), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3441, listloop_s3442)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3764, o19, listloop_i3441, listloop_s3442)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3769 = () =>
            {
                var smplCommandRemember3770 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3768 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3768.SetZero(smpl);

                foreach (IVariable listloop_s3766 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3768.InjectAdd(smpl, temp3768, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3767)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3766), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3766), O.Negate(smpl, i3767)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3766), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3766)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3770;
                return temp3768;

            };


            O.Assignment o20 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3765 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qI_inv_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func3769(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3771)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), new ScalarString("tot")), O.Negate(smpl, i3771)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3765, o20, new ScalarString("invt"), new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o21 = new O.Assignment();
            foreach (IVariable listloop_s3443 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o21)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3772 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3443), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_Inv_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3443), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3443), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3443));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3772, o21, new ScalarString("invt"), listloop_s3443)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o22 = new O.Assignment();
            foreach (IVariable listloop_s3444 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o22)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3773 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3444), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_Inv_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3444), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3444), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3444), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3444), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3444)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3773, o22, new ScalarString("invt"), listloop_s3444)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o23 = new O.Assignment();
            foreach (IVariable listloop_s3445 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o23)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3774 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3445), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_Inv_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3445), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3445), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sI_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3445), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3445), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3445)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3774, o23, new ScalarString("invt"), listloop_s3445)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o24 = new O.Assignment();
            foreach (IVariable listloop_i3446 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o24)))
            {
                foreach (IVariable listloop_s3447 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o24)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3775 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3446, listloop_s3447), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3446, listloop_s3447), O.Multiply(smpl, O.Add(smpl, i3776, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3446, listloop_s3447), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3446, listloop_s3447)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3447), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3447)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3775, o24, listloop_i3446, listloop_s3447)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o25 = new O.Assignment();
            foreach (IVariable listloop_i3448 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o25)))
            {
                foreach (IVariable listloop_s3449 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o25)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3777 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3448, listloop_s3449), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3448, listloop_s3449), O.Multiply(smpl, O.Add(smpl, i3778, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3448, listloop_s3449), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3448, listloop_s3449)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3449), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3449)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3777, o25, listloop_i3448, listloop_s3449)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o26 = new O.Assignment();
            foreach (IVariable listloop_i3450 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o26)))
            {
                foreach (IVariable listloop_s3451 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o26)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3779 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3450, listloop_s3451), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3450, listloop_s3451), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3450, listloop_s3451), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3450, listloop_s3451), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3450, listloop_s3451), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3450, listloop_s3451)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3779, o26, listloop_i3450, listloop_s3451)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o27 = new O.Assignment();
            foreach (IVariable listloop_i3452 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o27)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3780 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3452), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_i_tot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3452), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3452, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3452, new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3452, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3452, new ScalarString("tot"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3780, o27, listloop_i3452, new ScalarString("tot"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o28 = new O.Assignment();
            foreach (IVariable listloop_i3453 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o28)))
            {
                foreach (IVariable listloop_s3454 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o28)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3781 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3453, listloop_s3454), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3453, listloop_s3454), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3453, listloop_s3454), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fpI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3453, listloop_s3454), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3453, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3453, new ScalarString("tot"))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3781, o28, listloop_i3453, listloop_s3454)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o29 = new O.Assignment();
            foreach (IVariable listloop_s3455 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o29)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3782 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3455), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_inventory", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3455), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("invt"), listloop_s3455), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_s", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("invt"), listloop_s3455));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3782, o29, new ScalarString("invt"), listloop_s3455)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o30 = new O.Assignment();
            foreach (IVariable listloop_i3456 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o30)))
            {
                foreach (IVariable listloop_s3457 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o30)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3783 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3456, listloop_s3457), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3456, listloop_s3457), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3456, listloop_s3457), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3456, listloop_s3457), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3456, listloop_s3457), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3456, listloop_s3457)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3783, o30, listloop_i3456, listloop_s3457)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o31 = new O.Assignment();
            foreach (IVariable listloop_i3458 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o31)))
            {
                foreach (IVariable listloop_s3459 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o31)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3784 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3458, listloop_s3459), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3458, listloop_s3459), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3458, listloop_s3459), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3458, listloop_s3459), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3458, listloop_s3459), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3458, listloop_s3459)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3784, o31, listloop_i3458, listloop_s3459)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o32 = new O.Assignment();
            foreach (IVariable listloop_i3460 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o32)))
            {
                foreach (IVariable listloop_s3461 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o32)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3785 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3460, listloop_s3461), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3460, listloop_s3461), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3460, listloop_s3461), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3460, listloop_s3461)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3460, listloop_s3461), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3460, listloop_s3461));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3785, o32, listloop_i3460, listloop_s3461)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3789 = (IVariable listloop_i3462) =>
            {
                var smplCommandRemember3790 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3788 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3788.SetZero(smpl);

                foreach (IVariable listloop_s3787 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3788.InjectAdd(smpl, temp3788, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3462, listloop_s3787), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3462, listloop_s3787));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3790;
                return temp3788;

            };


            O.Assignment o33 = new O.Assignment();
            foreach (IVariable listloop_i3462 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o33)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3786 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3462), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI_i_tot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3462), func3789(listloop_i3462));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3786, o33, listloop_i3462, new ScalarString("tot"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o34 = new O.Assignment();
            foreach (IVariable listloop_i3463 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o34)))
            {
                foreach (IVariable listloop_s3464 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o34)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3791 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3463, listloop_s3464), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3463, listloop_s3464), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3463, listloop_s3464), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3463, listloop_s3464), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3463, listloop_s3464), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3463, listloop_s3464)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3791, o34, listloop_i3463, listloop_s3464)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3795 = (IVariable listloop_s3465) =>
            {
                var smplCommandRemember3796 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3794 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3794.SetZero(smpl);

                foreach (IVariable listloop_i3793 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3794.InjectAdd(smpl, temp3794, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3793, listloop_s3465), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3793, listloop_s3465));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3796;
                return temp3794;

            };


            O.Assignment o35 = new O.Assignment();
            foreach (IVariable listloop_s3465 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o35)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3792 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3465), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vI_tot_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3465), func3795(listloop_s3465));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3792, o35, new ScalarString("tot"), listloop_s3465)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3800 = () =>
            {
                var smplCommandRemember3801 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3799 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3799.SetZero(smpl);

                foreach (IVariable listloop_i3798 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3799.InjectAdd(smpl, temp3799, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3798, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3798, new ScalarString("tot")));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3801;
                return temp3799;

            };


            O.Assignment o36 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3797 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vI_tot_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3800());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3797, o36, new ScalarString("tot"), new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3806 = (IVariable listloop_s3466) =>
            {
                var smplCommandRemember3807 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3805 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3805.SetZero(smpl);

                foreach (IVariable listloop_i3803 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3805.InjectAdd(smpl, temp3805, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3804)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3803, listloop_s3466), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3803, listloop_s3466), O.Negate(smpl, i3804)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3803, listloop_s3466), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3803, listloop_s3466)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3807;
                return temp3805;

            };


            O.Assignment o37 = new O.Assignment();
            foreach (IVariable listloop_s3466 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o37)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3802 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3466), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_tot_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3466), O.Divide(smpl, func3806(listloop_s3466), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3808)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_s3466), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_s3466), O.Negate(smpl, i3808)
                )));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3802, o37, new ScalarString("tot"), listloop_s3466)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o38 = new O.Assignment();
            foreach (IVariable listloop_s3467 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o38)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3809 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3467), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pI_tot_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3467), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_s3467), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_s3467), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_s3467), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_s3467)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3809, o38, new ScalarString("tot"), listloop_s3467)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3814 = () =>
            {
                var smplCommandRemember3815 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3813 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3813.SetZero(smpl);

                foreach (IVariable listloop_i3811 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3813.InjectAdd(smpl, temp3813, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3812)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3811, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3811, new ScalarString("tot")), O.Negate(smpl, i3812)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3811, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3811, new ScalarString("tot"))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3815;
                return temp3813;

            };


            O.Assignment o39 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3810 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qI_tot_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func3814(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3816)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot")), O.Negate(smpl, i3816)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3810, o39, new ScalarString("tot"), new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o40 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3817 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pI_tot_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3817, o40, new ScalarString("tot"), new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o41 = new O.Assignment();
            foreach (IVariable listloop_r3468 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o41)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3818 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3468), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3468), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3468), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3468), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3468), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3468)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3818, o41, listloop_r3468)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o42 = new O.Assignment();
            foreach (IVariable listloop_r3469 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o42)))
            {
                foreach (IVariable listloop_s3470 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o42)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3819 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3469, listloop_s3470), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3469, listloop_s3470), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3469, listloop_s3470), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3469, listloop_s3470), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3469, listloop_s3470), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3469, listloop_s3470)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pR_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3819, o42, listloop_r3469, listloop_s3470)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o43 = new O.Assignment();
            foreach (IVariable listloop_r3471 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o43)))
            {
                foreach (IVariable listloop_s3472 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o43)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3820 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3471, listloop_s3472), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3471, listloop_s3472), O.Multiply(smpl, O.Add(smpl, i3821, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3471, listloop_s3472), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3471, listloop_s3472)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3472), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3472)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3820, o43, listloop_r3471, listloop_s3472)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o44 = new O.Assignment();
            foreach (IVariable listloop_r3473 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o44)))
            {
                foreach (IVariable listloop_s3474 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o44)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3822 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3473, listloop_s3474), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3473, listloop_s3474), O.Multiply(smpl, O.Add(smpl, i3823, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3473, listloop_s3474), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3473, listloop_s3474)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3474), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3474)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3822, o44, listloop_r3473, listloop_s3474)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o45 = new O.Assignment();
            foreach (IVariable listloop_r3475 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o45)))
            {
                foreach (IVariable listloop_s3476 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o45)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3824 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475, listloop_s3476), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475, listloop_s3476), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475, listloop_s3476), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475, listloop_s3476), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475, listloop_s3476), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475, listloop_s3476)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3475), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3475)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3824, o45, listloop_r3475, listloop_s3476)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o46 = new O.Assignment();
            foreach (IVariable listloop_r3477 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o46)))
            {
                foreach (IVariable listloop_s3478 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o46)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3825 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3477, listloop_s3478), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3477, listloop_s3478)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3825, o46, listloop_r3477, listloop_s3478)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o47 = new O.Assignment();
            foreach (IVariable listloop_r3479 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o47)))
            {
                foreach (IVariable listloop_s3480 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o47)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3826 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3479, listloop_s3480), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3479, listloop_s3480)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3826, o47, listloop_r3479, listloop_s3480)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3831 = () =>
            {
                var smplCommandRemember3832 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3830 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3830.SetZero(smpl);

                foreach (IVariable listloop_r3828 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3830.InjectAdd(smpl, temp3830, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3829)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3828), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3828), O.Negate(smpl, i3829)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3828), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3828)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3832;
                return temp3830;

            };


            O.Assignment o48 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3827 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qR_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func3831(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3833)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i3833)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3827, o48, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o49 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3834 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pR_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3834, o49, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o50 = new O.Assignment();
            foreach (IVariable listloop_r3481 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o50)))
            {
                foreach (IVariable listloop_s3482 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o50)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3835 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3481, listloop_s3482), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3481, listloop_s3482), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3481, listloop_s3482), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3481, listloop_s3482), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3481, listloop_s3482), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3481, listloop_s3482)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3835, o50, listloop_r3481, listloop_s3482)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o51 = new O.Assignment();
            foreach (IVariable listloop_r3483 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o51)))
            {
                foreach (IVariable listloop_s3484 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o51)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3836 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3483, listloop_s3484), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3483, listloop_s3484), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3483, listloop_s3484), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3483, listloop_s3484), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3483, listloop_s3484), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3483, listloop_s3484)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3836, o51, listloop_r3483, listloop_s3484)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o52 = new O.Assignment();
            foreach (IVariable listloop_r3485 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o52)))
            {
                foreach (IVariable listloop_s3486 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o52)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3837 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3485, listloop_s3486), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3485, listloop_s3486), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3485, listloop_s3486), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3485, listloop_s3486)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3485, listloop_s3486), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3485, listloop_s3486));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vR_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3837, o52, listloop_r3485, listloop_s3486)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3841 = (IVariable listloop_r3487) =>
            {
                var smplCommandRemember3842 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3840 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3840.SetZero(smpl);

                foreach (IVariable listloop_s3839 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3840.InjectAdd(smpl, temp3840, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3487, listloop_s3839), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3487, listloop_s3839));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3842;
                return temp3840;

            };


            O.Assignment o53 = new O.Assignment();
            foreach (IVariable listloop_r3487 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o53)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3838 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3487), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3487), func3841(listloop_r3487));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3838, o53, listloop_r3487)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3846 = () =>
            {
                var smplCommandRemember3847 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3845 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3845.SetZero(smpl);

                foreach (IVariable listloop_r3844 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3845.InjectAdd(smpl, temp3845, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3844), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3844));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3847;
                return temp3845;

            };


            O.Assignment o54 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3843 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vR_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3846());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3843, o54, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o55 = new O.Assignment();
            foreach (IVariable listloop_c_3488 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c_")))), null, new LookupSettings(), EVariableType.Var, o55)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3848 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3488), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3488), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3488), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3488), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3488), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3488)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3848, o55, listloop_c_3488)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o56 = new O.Assignment();
            foreach (IVariable listloop_c3489 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o56)))
            {
                foreach (IVariable listloop_s3490 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o56)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3849 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3489, listloop_s3490), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3489, listloop_s3490), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3489, listloop_s3490), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3489, listloop_s3490), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3489, listloop_s3490), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3489, listloop_s3490)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3849, o56, listloop_c3489, listloop_s3490)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o57 = new O.Assignment();
            foreach (IVariable listloop_c3491 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o57)))
            {
                foreach (IVariable listloop_s3492 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o57)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3850 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3491, listloop_s3492), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3491, listloop_s3492), O.Multiply(smpl, O.Add(smpl, i3851, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3491, listloop_s3492), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3491, listloop_s3492)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3492), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3492)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3850, o57, listloop_c3491, listloop_s3492)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o58 = new O.Assignment();
            foreach (IVariable listloop_c3493 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o58)))
            {
                foreach (IVariable listloop_s3494 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o58)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3852 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3493, listloop_s3494), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3493, listloop_s3494), O.Multiply(smpl, O.Add(smpl, i3853, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3493, listloop_s3494), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3493, listloop_s3494)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3494), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3494)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3852, o58, listloop_c3493, listloop_s3494)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o59 = new O.Assignment();
            foreach (IVariable listloop_c3495 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o59)))
            {
                foreach (IVariable listloop_sp3496 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o59)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3854 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495, listloop_sp3496), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495, listloop_sp3496), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495, listloop_sp3496), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495, listloop_sp3496), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495, listloop_sp3496), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495, listloop_sp3496)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495))), O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCTourist", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495)), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495, new ScalarString("PUB")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3495, listloop_sp3496), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3495, listloop_sp3496)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3854, o59, listloop_c3495, listloop_sp3496)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o60 = new O.Assignment();
            foreach (IVariable listloop_c3497 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o60)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3855 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3497), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC_s_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3497), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3497, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3497, new ScalarString("PUB")), O.Lookup(smpl, null, null, "vPublicSales", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3497, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3497, new ScalarString("PUB"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3855, o60, listloop_c3497, new ScalarString("PUB"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o61 = new O.Assignment();
            foreach (IVariable listloop_c3498 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o61)))
            {
                foreach (IVariable listloop_s3499 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o61)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3856 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3498, listloop_s3499), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3498, listloop_s3499)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3856, o61, listloop_c3498, listloop_s3499)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o62 = new O.Assignment();
            foreach (IVariable listloop_c3500 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o62)))
            {
                foreach (IVariable listloop_s3501 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o62)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3857 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3500, listloop_s3501), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3500, listloop_s3501)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3857, o62, listloop_c3500, listloop_s3501)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3861 = (IVariable listloop_c3502) =>
            {
                var smplCommandRemember3862 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3860 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3860.SetZero(smpl);

                foreach (IVariable listloop_s3859 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3860.InjectAdd(smpl, temp3860, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3502, listloop_s3859), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3502, listloop_s3859));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3862;
                return temp3860;

            };


            O.Assignment o63 = new O.Assignment();
            foreach (IVariable listloop_c3502 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o63)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3858 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3502), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3502), func3861(listloop_c3502)), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3502), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3502), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3502), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCTourist", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3502)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3858, o63, listloop_c3502)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o64 = new O.Assignment();
            foreach (IVariable listloop_c3503 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o64)))
            {
                foreach (IVariable listloop_s3504 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o64)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3863 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3503, listloop_s3504), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3503, listloop_s3504), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3503, listloop_s3504), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3503, listloop_s3504), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3503, listloop_s3504), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3503, listloop_s3504)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3863, o64, listloop_c3503, listloop_s3504)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o65 = new O.Assignment();
            foreach (IVariable listloop_c3505 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o65)))
            {
                foreach (IVariable listloop_s3506 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o65)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3864 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3505, listloop_s3506), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3505, listloop_s3506), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3505, listloop_s3506), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3505, listloop_s3506), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3505, listloop_s3506), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3505, listloop_s3506)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3864, o65, listloop_c3505, listloop_s3506)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o66 = new O.Assignment();
            foreach (IVariable listloop_c3507 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o66)))
            {
                foreach (IVariable listloop_s3508 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o66)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3865 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3507, listloop_s3508), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vC_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3507, listloop_s3508), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3507, listloop_s3508), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3507, listloop_s3508)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3507, listloop_s3508), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3507, listloop_s3508));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vC_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3865, o66, listloop_c3507, listloop_s3508)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o67 = new O.Assignment();
            foreach (IVariable listloop_g_3509 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g_")))), null, new LookupSettings(), EVariableType.Var, o67)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3866 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3509), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3509), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3509), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3509), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3509), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3509)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3866, o67, listloop_g_3509)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o68 = new O.Assignment();
            foreach (IVariable listloop_g3510 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o68)))
            {
                foreach (IVariable listloop_s3511 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o68)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3867 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3510, listloop_s3511), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3510, listloop_s3511), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3510, listloop_s3511), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3510, listloop_s3511), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3510, listloop_s3511), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3510, listloop_s3511)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pG_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3867, o68, listloop_g3510, listloop_s3511)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o69 = new O.Assignment();
            foreach (IVariable listloop_g3512 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o69)))
            {
                foreach (IVariable listloop_s3513 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o69)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3868 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3512, listloop_s3513), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3512, listloop_s3513), O.Multiply(smpl, O.Add(smpl, i3869, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3512, listloop_s3513), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3512, listloop_s3513)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3513), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3513)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3868, o69, listloop_g3512, listloop_s3513)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o70 = new O.Assignment();
            foreach (IVariable listloop_g3514 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o70)))
            {
                foreach (IVariable listloop_s3515 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o70)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3870 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3514, listloop_s3515), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3514, listloop_s3515), O.Multiply(smpl, O.Add(smpl, i3871, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3514, listloop_s3515), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3514, listloop_s3515)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3515), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3515)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3870, o70, listloop_g3514, listloop_s3515)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o71 = new O.Assignment();
            foreach (IVariable listloop_g3516 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o71)))
            {
                foreach (IVariable listloop_s3517 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o71)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3872 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516, listloop_s3517), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516, listloop_s3517), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516, listloop_s3517), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516, listloop_s3517), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516, listloop_s3517), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516, listloop_s3517)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3516), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3516)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qG_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3872, o71, listloop_g3516, listloop_s3517)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o72 = new O.Assignment();
            foreach (IVariable listloop_g3518 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o72)))
            {
                foreach (IVariable listloop_s3519 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o72)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3873 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3518, listloop_s3519), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3518, listloop_s3519)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3873, o72, listloop_g3518, listloop_s3519)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o73 = new O.Assignment();
            foreach (IVariable listloop_g3520 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o73)))
            {
                foreach (IVariable listloop_s3521 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o73)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3874 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3520, listloop_s3521), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3520, listloop_s3521)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3874, o73, listloop_g3520, listloop_s3521)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3878 = (IVariable listloop_g3522) =>
            {
                var smplCommandRemember3879 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3877 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3877.SetZero(smpl);

                foreach (IVariable listloop_s3876 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3877.InjectAdd(smpl, temp3877, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3522, listloop_s3876), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3522, listloop_s3876));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3879;
                return temp3877;

            };


            O.Assignment o74 = new O.Assignment();
            foreach (IVariable listloop_g3522 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o74)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3875 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3522), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3522), func3878(listloop_g3522));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3875, o74, listloop_g3522)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o75 = new O.Assignment();
            foreach (IVariable listloop_g3523 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o75)))
            {
                foreach (IVariable listloop_s3524 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o75)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3880 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3523, listloop_s3524), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vG_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3523, listloop_s3524), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3523, listloop_s3524), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3523, listloop_s3524)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3523, listloop_s3524), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3523, listloop_s3524));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vG_s", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3880, o75, listloop_g3523, listloop_s3524)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o76 = new O.Assignment();
            foreach (IVariable listloop_g3525 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o76)))
            {
                foreach (IVariable listloop_s3526 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o76)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3881 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3525, listloop_s3526), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3525, listloop_s3526), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3525, listloop_s3526), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3525, listloop_s3526), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3525, listloop_s3526), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3525, listloop_s3526)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3881, o76, listloop_g3525, listloop_s3526)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o77 = new O.Assignment();
            foreach (IVariable listloop_g3527 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o77)))
            {
                foreach (IVariable listloop_s3528 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o77)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3882 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3527, listloop_s3528), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3527, listloop_s3528), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3527, listloop_s3528), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3527, listloop_s3528), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3527, listloop_s3528), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3527, listloop_s3528)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3882, o77, listloop_g3527, listloop_s3528)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o78 = new O.Assignment();
            foreach (IVariable listloop_c3529 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o78)))
            {
                foreach (IVariable listloop_s3530 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o78)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3883 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3529, listloop_s3530), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3529, listloop_s3530), O.Multiply(smpl, O.Add(smpl, i3884, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3529, listloop_s3530), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3529, listloop_s3530)), O.Add(smpl, i3885, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3529, listloop_s3530), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3529, listloop_s3530)))), i3886);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3883, o78, listloop_c3529, listloop_s3530)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o79 = new O.Assignment();
            foreach (IVariable listloop_c3531 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o79)))
            {
                foreach (IVariable listloop_s3532 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o79)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3887 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3531, listloop_s3532), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3531, listloop_s3532), O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i3888, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3531, listloop_s3532), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3531, listloop_s3532)), O.Add(smpl, i3889, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3531, listloop_s3532), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3531, listloop_s3532))), O.Add(smpl, i3890, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3531, listloop_s3532), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3531, listloop_s3532)))), i3891);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3887, o79, listloop_c3531, listloop_s3532)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o80 = new O.Assignment();
            foreach (IVariable listloop_g3533 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o80)))
            {
                foreach (IVariable listloop_s3534 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o80)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3892 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3533, listloop_s3534), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3533, listloop_s3534), O.Multiply(smpl, O.Add(smpl, i3893, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3533, listloop_s3534), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3533, listloop_s3534)), O.Add(smpl, i3894, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3533, listloop_s3534), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3533, listloop_s3534)))), i3895);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3892, o80, listloop_g3533, listloop_s3534)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o81 = new O.Assignment();
            foreach (IVariable listloop_g3535 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o81)))
            {
                foreach (IVariable listloop_s3536 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o81)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3896 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3535, listloop_s3536), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3535, listloop_s3536), O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i3897, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3535, listloop_s3536), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3535, listloop_s3536)), O.Add(smpl, i3898, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3535, listloop_s3536), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3535, listloop_s3536))), O.Add(smpl, i3899, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3535, listloop_s3536), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3535, listloop_s3536)))), i3900);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3896, o81, listloop_g3535, listloop_s3536)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o82 = new O.Assignment();
            foreach (IVariable listloop_x3537 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o82)))
            {
                foreach (IVariable listloop_s3538 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o82)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3901 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3537, listloop_s3538), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3537, listloop_s3538), O.Multiply(smpl, O.Add(smpl, i3902, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3537, listloop_s3538), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3537, listloop_s3538)), O.Add(smpl, i3903, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3537, listloop_s3538), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3537, listloop_s3538)))), i3904);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3901, o82, listloop_x3537, listloop_s3538)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o83 = new O.Assignment();
            foreach (IVariable listloop_x3539 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o83)))
            {
                foreach (IVariable listloop_s3540 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o83)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3905 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3539, listloop_s3540), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3539, listloop_s3540), O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i3906, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3539, listloop_s3540), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3539, listloop_s3540)), O.Add(smpl, i3907, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3539, listloop_s3540), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3539, listloop_s3540))), O.Add(smpl, i3908, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3539, listloop_s3540), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3539, listloop_s3540)))), i3909);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3905, o83, listloop_x3539, listloop_s3540)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o84 = new O.Assignment();
            foreach (IVariable listloop_r3541 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o84)))
            {
                foreach (IVariable listloop_s3542 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o84)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3910 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3541, listloop_s3542), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3541, listloop_s3542), O.Multiply(smpl, O.Add(smpl, i3911, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3541, listloop_s3542), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3541, listloop_s3542)), O.Add(smpl, i3912, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3541, listloop_s3542), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3541, listloop_s3542)))), i3913);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3910, o84, listloop_r3541, listloop_s3542)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o85 = new O.Assignment();
            foreach (IVariable listloop_r3543 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o85)))
            {
                foreach (IVariable listloop_s3544 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o85)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3914 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3543, listloop_s3544), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3543, listloop_s3544), O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i3915, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3543, listloop_s3544), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3543, listloop_s3544)), O.Add(smpl, i3916, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3543, listloop_s3544), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3543, listloop_s3544))), O.Add(smpl, i3917, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3543, listloop_s3544), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3543, listloop_s3544)))), i3918);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3914, o85, listloop_r3543, listloop_s3544)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o86 = new O.Assignment();
            foreach (IVariable listloop_i3545 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o86)))
            {
                foreach (IVariable listloop_s3546 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o86)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3919 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3545, listloop_s3546), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3545, listloop_s3546), O.Multiply(smpl, O.Add(smpl, i3920, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3545, listloop_s3546), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3545, listloop_s3546)), O.Add(smpl, i3921, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3545, listloop_s3546), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3545, listloop_s3546)))), i3922);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3919, o86, listloop_i3545, listloop_s3546)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o87 = new O.Assignment();
            foreach (IVariable listloop_i3547 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o87)))
            {
                foreach (IVariable listloop_s3548 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o87)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar3923 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3547, listloop_s3548), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3547, listloop_s3548), O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i3924, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3547, listloop_s3548), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3547, listloop_s3548)), O.Add(smpl, i3925, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3547, listloop_s3548), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3547, listloop_s3548))), O.Add(smpl, i3926, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3547, listloop_s3548), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3547, listloop_s3548)))), i3927);
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3923, o87, listloop_i3547, listloop_s3548)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3932 = (IVariable listloop_c3549) =>
            {
                var smplCommandRemember3933 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3931 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3931.SetZero(smpl);

                foreach (IVariable listloop_s3929 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3931.InjectAdd(smpl, temp3931, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3549, listloop_s3929), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3549, listloop_s3929), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3549, listloop_s3929), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3549, listloop_s3929)), O.Add(smpl, i3930, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3549, listloop_s3929), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3549, listloop_s3929))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3933;
                return temp3931;

            };


            O.Assignment o88 = new O.Assignment();
            foreach (IVariable listloop_c3549 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o88)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3928 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3549), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3549), func3932(listloop_c3549));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3928, o88, listloop_c3549)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3938 = (IVariable listloop_c3550) =>
            {
                var smplCommandRemember3939 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3937 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3937.SetZero(smpl);

                foreach (IVariable listloop_s3935 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3937.InjectAdd(smpl, temp3937, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3550, listloop_s3935), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3550, listloop_s3935), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3550, listloop_s3935), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3550, listloop_s3935)), O.Add(smpl, i3936, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3550, listloop_s3935), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3550, listloop_s3935))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3939;
                return temp3937;

            };


            O.Assignment o89 = new O.Assignment();
            foreach (IVariable listloop_c3550 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o89)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3934 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3550), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3550), func3938(listloop_c3550));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3934, o89, listloop_c3550)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3943 = () =>
            {
                var smplCommandRemember3944 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3942 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3942.SetZero(smpl);

                foreach (IVariable listloop_c3941 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3942.InjectAdd(smpl, temp3942, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3941), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3941));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3944;
                return temp3942;

            };


            O.Assignment o90 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3940 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtC_y_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3943());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3940, o90, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3948 = () =>
            {
                var smplCommandRemember3949 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3947 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3947.SetZero(smpl);

                foreach (IVariable listloop_c3946 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3947.InjectAdd(smpl, temp3947, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3946), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3946));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3949;
                return temp3947;

            };


            O.Assignment o91 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3945 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtC_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3948());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3945, o91, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o92 = new O.Assignment();
            foreach (IVariable listloop_c3551 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o92)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3950 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3551), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3551), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3551), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3551)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3551), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3551));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3950, o92, listloop_c3551)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o93 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3951 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtC_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3951, o93, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3956 = (IVariable listloop_g3552) =>
            {
                var smplCommandRemember3957 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3955 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3955.SetZero(smpl);

                foreach (IVariable listloop_s3953 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3955.InjectAdd(smpl, temp3955, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3552, listloop_s3953), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3552, listloop_s3953), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3552, listloop_s3953), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3552, listloop_s3953)), O.Add(smpl, i3954, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3552, listloop_s3953), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3552, listloop_s3953))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3957;
                return temp3955;

            };


            O.Assignment o94 = new O.Assignment();
            foreach (IVariable listloop_g3552 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o94)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3952 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3552), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3552), func3956(listloop_g3552));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3952, o94, listloop_g3552)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3962 = (IVariable listloop_g3553) =>
            {
                var smplCommandRemember3963 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3961 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3961.SetZero(smpl);

                foreach (IVariable listloop_s3959 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3961.InjectAdd(smpl, temp3961, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3553, listloop_s3959), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3553, listloop_s3959), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3553, listloop_s3959), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3553, listloop_s3959)), O.Add(smpl, i3960, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3553, listloop_s3959), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3553, listloop_s3959))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3963;
                return temp3961;

            };


            O.Assignment o95 = new O.Assignment();
            foreach (IVariable listloop_g3553 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o95)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3958 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3553), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3553), func3962(listloop_g3553));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3958, o95, listloop_g3553)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3967 = () =>
            {
                var smplCommandRemember3968 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3966 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3966.SetZero(smpl);

                foreach (IVariable listloop_g3965 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3966.InjectAdd(smpl, temp3966, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3965), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3965));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3968;
                return temp3966;

            };


            O.Assignment o96 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3964 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtG_y_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3967());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3964, o96, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3972 = () =>
            {
                var smplCommandRemember3973 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3971 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3971.SetZero(smpl);

                foreach (IVariable listloop_g3970 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3971.InjectAdd(smpl, temp3971, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3970), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3970));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3973;
                return temp3971;

            };


            O.Assignment o97 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3969 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtG_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3972());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3969, o97, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o98 = new O.Assignment();
            foreach (IVariable listloop_g3554 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o98)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3974 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3554), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3554), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3554), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3554)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3554), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3554));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3974, o98, listloop_g3554)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o99 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3975 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtG_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3975, o99, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3980 = (IVariable listloop_r3555) =>
            {
                var smplCommandRemember3981 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3979 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3979.SetZero(smpl);

                foreach (IVariable listloop_s3977 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3979.InjectAdd(smpl, temp3979, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3555, listloop_s3977), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3555, listloop_s3977), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3555, listloop_s3977), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3555, listloop_s3977)), O.Add(smpl, i3978, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3555, listloop_s3977), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3555, listloop_s3977))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3981;
                return temp3979;

            };


            O.Assignment o100 = new O.Assignment();
            foreach (IVariable listloop_r3555 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o100)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3976 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3555), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3555), func3980(listloop_r3555));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3976, o100, listloop_r3555)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func3986 = (IVariable listloop_r3556) =>
            {
                var smplCommandRemember3987 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3985 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3985.SetZero(smpl);

                foreach (IVariable listloop_s3983 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3985.InjectAdd(smpl, temp3985, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3556, listloop_s3983), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3556, listloop_s3983), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3556, listloop_s3983), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3556, listloop_s3983)), O.Add(smpl, i3984, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3556, listloop_s3983), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3556, listloop_s3983))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3987;
                return temp3985;

            };


            O.Assignment o101 = new O.Assignment();
            foreach (IVariable listloop_r3556 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o101)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3982 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3556), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3556), func3986(listloop_r3556));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3982, o101, listloop_r3556)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3991 = () =>
            {
                var smplCommandRemember3992 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3990 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3990.SetZero(smpl);

                foreach (IVariable listloop_r3989 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3990.InjectAdd(smpl, temp3990, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3989), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3989));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3992;
                return temp3990;

            };


            O.Assignment o102 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3988 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtR_y_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3991());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3988, o102, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func3996 = () =>
            {
                var smplCommandRemember3997 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp3995 = new Series(ESeriesType.Normal, Program.options.freq, null); temp3995.SetZero(smpl);

                foreach (IVariable listloop_r3994 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp3995.InjectAdd(smpl, temp3995, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3994), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3994));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember3997;
                return temp3995;

            };


            O.Assignment o103 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3993 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtR_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func3996());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3993, o103, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o104 = new O.Assignment();
            foreach (IVariable listloop_r3557 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o104)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar3998 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3557), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3557), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3557), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3557)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3557), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3557));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3998, o104, listloop_r3557)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o105 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3999 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtR_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar3999, o105, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4004 = (IVariable listloop_x3558) =>
            {
                var smplCommandRemember4005 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4003 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4003.SetZero(smpl);

                foreach (IVariable listloop_s4001 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4003.InjectAdd(smpl, temp4003, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3558, listloop_s4001), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3558, listloop_s4001), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3558, listloop_s4001), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3558, listloop_s4001)), O.Add(smpl, i4002, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3558, listloop_s4001), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3558, listloop_s4001))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4005;
                return temp4003;

            };


            O.Assignment o106 = new O.Assignment();
            foreach (IVariable listloop_x3558 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o106)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4000 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3558), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3558), func4004(listloop_x3558));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4000, o106, listloop_x3558)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4010 = (IVariable listloop_x3559) =>
            {
                var smplCommandRemember4011 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4009 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4009.SetZero(smpl);

                foreach (IVariable listloop_s4007 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4009.InjectAdd(smpl, temp4009, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3559, listloop_s4007), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3559, listloop_s4007), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3559, listloop_s4007), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3559, listloop_s4007)), O.Add(smpl, i4008, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3559, listloop_s4007), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3559, listloop_s4007))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4011;
                return temp4009;

            };


            O.Assignment o107 = new O.Assignment();
            foreach (IVariable listloop_x3559 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o107)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4006 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3559), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3559), func4010(listloop_x3559));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4006, o107, listloop_x3559)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4015 = () =>
            {
                var smplCommandRemember4016 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4014 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4014.SetZero(smpl);

                foreach (IVariable listloop_x4013 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4014.InjectAdd(smpl, temp4014, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4013), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4013));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4016;
                return temp4014;

            };


            O.Assignment o108 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4012 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtX_y_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4015());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4012, o108, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4020 = () =>
            {
                var smplCommandRemember4021 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4019 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4019.SetZero(smpl);

                foreach (IVariable listloop_x4018 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4019.InjectAdd(smpl, temp4019, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4018), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4018));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4021;
                return temp4019;

            };


            O.Assignment o109 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4017 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtX_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4020());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4017, o109, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o110 = new O.Assignment();
            foreach (IVariable listloop_x3560 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o110)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4022 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3560), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3560), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3560), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3560)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3560), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3560));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4022, o110, listloop_x3560)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o111 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4023 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtX_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4023, o111, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4028 = (IVariable listloop_i3561) =>
            {
                var smplCommandRemember4029 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4027 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4027.SetZero(smpl);

                foreach (IVariable listloop_s4025 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4027.InjectAdd(smpl, temp4027, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3561, listloop_s4025), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3561, listloop_s4025), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3561, listloop_s4025), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3561, listloop_s4025)), O.Add(smpl, i4026, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3561, listloop_s4025), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3561, listloop_s4025))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4029;
                return temp4027;

            };


            O.Assignment o112 = new O.Assignment();
            foreach (IVariable listloop_i3561 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o112)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4024 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3561), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3561), func4028(listloop_i3561));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4024, o112, listloop_i3561)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4034 = (IVariable listloop_i3562) =>
            {
                var smplCommandRemember4035 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4033 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4033.SetZero(smpl);

                foreach (IVariable listloop_s4031 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4033.InjectAdd(smpl, temp4033, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3562, listloop_s4031), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3562, listloop_s4031), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3562, listloop_s4031), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3562, listloop_s4031)), O.Add(smpl, i4032, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3562, listloop_s4031), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3562, listloop_s4031))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4035;
                return temp4033;

            };


            O.Assignment o113 = new O.Assignment();
            foreach (IVariable listloop_i3562 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o113)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4030 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3562), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3562), func4034(listloop_i3562));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4030, o113, listloop_i3562)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4039 = () =>
            {
                var smplCommandRemember4040 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4038 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4038.SetZero(smpl);

                foreach (IVariable listloop_i4037 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4038.InjectAdd(smpl, temp4038, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4037), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4037));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4040;
                return temp4038;

            };


            O.Assignment o114 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4036 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtI_y_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4039());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4036, o114, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4044 = () =>
            {
                var smplCommandRemember4045 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4043 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4043.SetZero(smpl);

                foreach (IVariable listloop_i4042 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4043.InjectAdd(smpl, temp4043, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4042), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4042));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4045;
                return temp4043;

            };


            O.Assignment o115 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4041 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtI_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4044());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4041, o115, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o116 = new O.Assignment();
            foreach (IVariable listloop_i3563 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o116)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4046 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3563), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3563), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3563), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3563)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3563), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3563));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4046, o116, listloop_i3563)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o117 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4047 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtI_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4047, o117, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4052 = () =>
            {
                var smplCommandRemember4053 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4051 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4051.SetZero(smpl);

                foreach (IVariable listloop_s4049 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4051.InjectAdd(smpl, temp4051, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4049), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4049), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4049), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4049)), O.Add(smpl, i4050, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4049), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4049))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4053;
                return temp4051;

            };


            O.Assignment o118 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4048 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtY", null, null, new LookupSettings(), EVariableType.Var, null), func4052());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtY", null, ivTmpvar4048, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o118)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o119 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4054 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vDutyVAT_y", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vDutyVAT_y", null, ivTmpvar4054, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o119)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o120 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4055 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vDutyVAT_m", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtC_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtG_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtX_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtI_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtR_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vDutyVAT_m", null, ivTmpvar4055, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o120)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o121 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4056 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vDutyVAT", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vDutyVAT_y", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vDutyVAT_m", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vDutyVAT", null, ivTmpvar4056, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o121)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4062 = (IVariable listloop_c3564) =>
            {
                var smplCommandRemember4063 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4061 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4061.SetZero(smpl);

                foreach (IVariable listloop_s4058 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4061.InjectAdd(smpl, temp4061, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058)), O.Add(smpl, i4059, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058)), O.Add(smpl, i4060, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564, listloop_s4058), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564, listloop_s4058)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4063;
                return temp4061;

            };


            O.Assignment o122 = new O.Assignment();
            foreach (IVariable listloop_c3564 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o122)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4057 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3564), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3564), func4062(listloop_c3564));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4057, o122, listloop_c3564)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4067 = () =>
            {
                var smplCommandRemember4068 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4066 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4066.SetZero(smpl);

                foreach (IVariable listloop_c4065 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4066.InjectAdd(smpl, temp4066, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c4065), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c4065));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4068;
                return temp4066;

            };


            O.Assignment o123 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4064 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCVAT_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4067());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4064, o123, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4074 = (IVariable listloop_g3565) =>
            {
                var smplCommandRemember4075 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4073 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4073.SetZero(smpl);

                foreach (IVariable listloop_s4070 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4073.InjectAdd(smpl, temp4073, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070)), O.Add(smpl, i4071, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070)), O.Add(smpl, i4072, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565, listloop_s4070), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565, listloop_s4070)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4075;
                return temp4073;

            };


            O.Assignment o124 = new O.Assignment();
            foreach (IVariable listloop_g3565 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o124)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4069 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3565), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtGVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3565), func4074(listloop_g3565));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4069, o124, listloop_g3565)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4079 = () =>
            {
                var smplCommandRemember4080 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4078 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4078.SetZero(smpl);

                foreach (IVariable listloop_g4077 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4078.InjectAdd(smpl, temp4078, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4077), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4077));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4080;
                return temp4078;

            };


            O.Assignment o125 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4076 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtGVAT_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4079());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4076, o125, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4086 = (IVariable listloop_x3566) =>
            {
                var smplCommandRemember4087 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4085 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4085.SetZero(smpl);

                foreach (IVariable listloop_s4082 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4085.InjectAdd(smpl, temp4085, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082)), O.Add(smpl, i4083, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082)), O.Add(smpl, i4084, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566, listloop_s4082), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566, listloop_s4082)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4087;
                return temp4085;

            };


            O.Assignment o126 = new O.Assignment();
            foreach (IVariable listloop_x3566 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o126)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4081 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3566), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtXVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3566), func4086(listloop_x3566));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4081, o126, listloop_x3566)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4091 = () =>
            {
                var smplCommandRemember4092 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4090 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4090.SetZero(smpl);

                foreach (IVariable listloop_x4089 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4090.InjectAdd(smpl, temp4090, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4089), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4089));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4092;
                return temp4090;

            };


            O.Assignment o127 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4088 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtXVAT_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4091());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4088, o127, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4098 = (IVariable listloop_r3567) =>
            {
                var smplCommandRemember4099 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4097 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4097.SetZero(smpl);

                foreach (IVariable listloop_s4094 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4097.InjectAdd(smpl, temp4097, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094)), O.Add(smpl, i4095, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094)), O.Add(smpl, i4096, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567, listloop_s4094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567, listloop_s4094)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4099;
                return temp4097;

            };


            O.Assignment o128 = new O.Assignment();
            foreach (IVariable listloop_r3567 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o128)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4093 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3567), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtRVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3567), func4098(listloop_r3567));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4093, o128, listloop_r3567)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4103 = () =>
            {
                var smplCommandRemember4104 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4102 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4102.SetZero(smpl);

                foreach (IVariable listloop_r4101 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4102.InjectAdd(smpl, temp4102, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r4101), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r4101));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4104;
                return temp4102;

            };


            O.Assignment o129 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4100 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRVAT_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4103());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4100, o129, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4110 = (IVariable listloop_i3568) =>
            {
                var smplCommandRemember4111 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4109 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4109.SetZero(smpl);

                foreach (IVariable listloop_s4106 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4109.InjectAdd(smpl, temp4109, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106)), O.Add(smpl, i4107, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106)), O.Add(smpl, i4108, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568, listloop_s4106), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tVAT_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568, listloop_s4106)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4111;
                return temp4109;

            };


            O.Assignment o130 = new O.Assignment();
            foreach (IVariable listloop_i3568 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o130)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4105 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3568), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtIVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3568), func4110(listloop_i3568));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4105, o130, listloop_i3568)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4115 = () =>
            {
                var smplCommandRemember4116 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4114 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4114.SetZero(smpl);

                foreach (IVariable listloop_i4113 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4114.InjectAdd(smpl, temp4114, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4113), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIVAT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4113));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4116;
                return temp4114;

            };


            O.Assignment o131 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4112 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtIVAT_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4115());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIVAT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4112, o131, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o132 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4117 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtVAT", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCVAT", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGVAT", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXVAT", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIVAT", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRVAT", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtVAT", null, ivTmpvar4117, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o132)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4124 = (IVariable listloop_c3569) =>
            {
                var smplCommandRemember4125 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4123 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4123.SetZero(smpl);

                foreach (IVariable listloop_s4119 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4123.InjectAdd(smpl, temp4123, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119)), O.Add(smpl, i4120, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4121, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119)), O.Add(smpl, i4122, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569, listloop_s4119), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569, listloop_s4119)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4125;
                return temp4123;

            };


            O.Assignment o133 = new O.Assignment();
            foreach (IVariable listloop_c3569 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o133)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4118 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3569), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3569), func4124(listloop_c3569));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4118, o133, listloop_c3569)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4129 = () =>
            {
                var smplCommandRemember4130 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4128 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4128.SetZero(smpl);

                foreach (IVariable listloop_c4127 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4128.InjectAdd(smpl, temp4128, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c4127), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c4127));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4130;
                return temp4128;

            };


            O.Assignment o134 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4126 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4129());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4126, o134, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4137 = (IVariable listloop_g3570) =>
            {
                var smplCommandRemember4138 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4136 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4136.SetZero(smpl);

                foreach (IVariable listloop_s4132 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4136.InjectAdd(smpl, temp4136, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132)), O.Add(smpl, i4133, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4134, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132)), O.Add(smpl, i4135, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570, listloop_s4132), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570, listloop_s4132)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4138;
                return temp4136;

            };


            O.Assignment o135 = new O.Assignment();
            foreach (IVariable listloop_g3570 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o135)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4131 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3570), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtGDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3570), func4137(listloop_g3570));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4131, o135, listloop_g3570)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4142 = () =>
            {
                var smplCommandRemember4143 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4141 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4141.SetZero(smpl);

                foreach (IVariable listloop_g4140 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4141.InjectAdd(smpl, temp4141, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4140), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4140));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4143;
                return temp4141;

            };


            O.Assignment o136 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4139 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtGDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4142());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4139, o136, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4150 = (IVariable listloop_x3571) =>
            {
                var smplCommandRemember4151 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4149 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4149.SetZero(smpl);

                foreach (IVariable listloop_s4145 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4149.InjectAdd(smpl, temp4149, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145)), O.Add(smpl, i4146, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4147, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145)), O.Add(smpl, i4148, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571, listloop_s4145), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571, listloop_s4145)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4151;
                return temp4149;

            };


            O.Assignment o137 = new O.Assignment();
            foreach (IVariable listloop_x3571 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o137)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4144 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3571), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtXDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3571), func4150(listloop_x3571));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4144, o137, listloop_x3571)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4155 = () =>
            {
                var smplCommandRemember4156 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4154 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4154.SetZero(smpl);

                foreach (IVariable listloop_x4153 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4154.InjectAdd(smpl, temp4154, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4153), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4153));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4156;
                return temp4154;

            };


            O.Assignment o138 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4152 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtXDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4155());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4152, o138, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4163 = (IVariable listloop_r3572) =>
            {
                var smplCommandRemember4164 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4162 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4162.SetZero(smpl);

                foreach (IVariable listloop_s4158 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4162.InjectAdd(smpl, temp4162, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158)), O.Add(smpl, i4159, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4160, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158)), O.Add(smpl, i4161, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572, listloop_s4158), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572, listloop_s4158)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4164;
                return temp4162;

            };


            O.Assignment o139 = new O.Assignment();
            foreach (IVariable listloop_r3572 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o139)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4157 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3572), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtRDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3572), func4163(listloop_r3572));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4157, o139, listloop_r3572)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4168 = () =>
            {
                var smplCommandRemember4169 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4167 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4167.SetZero(smpl);

                foreach (IVariable listloop_r4166 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4167.InjectAdd(smpl, temp4167, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r4166), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r4166));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4169;
                return temp4167;

            };


            O.Assignment o140 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4165 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4168());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4165, o140, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4176 = (IVariable listloop_i3573) =>
            {
                var smplCommandRemember4177 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4175 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4175.SetZero(smpl);

                foreach (IVariable listloop_s4171 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4175.InjectAdd(smpl, temp4175, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171)), O.Add(smpl, i4172, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4173, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tDuty_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171)), O.Add(smpl, i4174, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573, listloop_s4171), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573, listloop_s4171)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4177;
                return temp4175;

            };


            O.Assignment o141 = new O.Assignment();
            foreach (IVariable listloop_i3573 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o141)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4170 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3573), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtIDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3573), func4176(listloop_i3573));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4170, o141, listloop_i3573)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4181 = () =>
            {
                var smplCommandRemember4182 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4180 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4180.SetZero(smpl);

                foreach (IVariable listloop_i4179 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4180.InjectAdd(smpl, temp4180, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4179), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4179));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4182;
                return temp4180;

            };


            O.Assignment o142 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4178 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtIDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4181());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4178, o142, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o143 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4183 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtDuty", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtDuty", null, ivTmpvar4183, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o143)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4190 = (IVariable listloop_c3574) =>
            {
                var smplCommandRemember4191 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4189 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4189.SetZero(smpl);

                foreach (IVariable listloop_s4185 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4189.InjectAdd(smpl, temp4189, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_C_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185)), O.Add(smpl, i4186, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4187, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185)), O.Add(smpl, i4188, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574, listloop_s4185), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574, listloop_s4185)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4191;
                return temp4189;

            };


            O.Assignment o144 = new O.Assignment();
            foreach (IVariable listloop_c3574 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o144)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4184 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3574), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3574), func4190(listloop_c3574));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4184, o144, listloop_c3574)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4195 = () =>
            {
                var smplCommandRemember4196 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4194 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4194.SetZero(smpl);

                foreach (IVariable listloop_c4193 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4194.InjectAdd(smpl, temp4194, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c4193), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c4193));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4196;
                return temp4194;

            };


            O.Assignment o145 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4192 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCExciseDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4195());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4192, o145, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4203 = (IVariable listloop_g3575) =>
            {
                var smplCommandRemember4204 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4202 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4202.SetZero(smpl);

                foreach (IVariable listloop_s4198 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4202.InjectAdd(smpl, temp4202, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_G_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198)), O.Add(smpl, i4199, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4200, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198)), O.Add(smpl, i4201, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575, listloop_s4198), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575, listloop_s4198)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4204;
                return temp4202;

            };


            O.Assignment o146 = new O.Assignment();
            foreach (IVariable listloop_g3575 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o146)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4197 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3575), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtGExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3575), func4203(listloop_g3575));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4197, o146, listloop_g3575)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4208 = () =>
            {
                var smplCommandRemember4209 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4207 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4207.SetZero(smpl);

                foreach (IVariable listloop_g4206 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4207.InjectAdd(smpl, temp4207, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4206), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4206));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4209;
                return temp4207;

            };


            O.Assignment o147 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4205 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtGExciseDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4208());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtGExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4205, o147, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4216 = (IVariable listloop_x3576) =>
            {
                var smplCommandRemember4217 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4215 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4215.SetZero(smpl);

                foreach (IVariable listloop_s4211 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4215.InjectAdd(smpl, temp4215, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211)), O.Add(smpl, i4212, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4213, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211)), O.Add(smpl, i4214, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576, listloop_s4211), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576, listloop_s4211)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4217;
                return temp4215;

            };


            O.Assignment o148 = new O.Assignment();
            foreach (IVariable listloop_x3576 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o148)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4210 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3576), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtXExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3576), func4216(listloop_x3576));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4210, o148, listloop_x3576)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4221 = () =>
            {
                var smplCommandRemember4222 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4220 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4220.SetZero(smpl);

                foreach (IVariable listloop_x4219 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4220.InjectAdd(smpl, temp4220, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4219), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4219));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4222;
                return temp4220;

            };


            O.Assignment o149 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4218 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtXExciseDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4221());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtXExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4218, o149, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4229 = (IVariable listloop_r3577) =>
            {
                var smplCommandRemember4230 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4228 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4228.SetZero(smpl);

                foreach (IVariable listloop_s4224 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4228.InjectAdd(smpl, temp4228, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224)), O.Add(smpl, i4225, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4226, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224)), O.Add(smpl, i4227, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577, listloop_s4224), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577, listloop_s4224)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4230;
                return temp4228;

            };


            O.Assignment o150 = new O.Assignment();
            foreach (IVariable listloop_r3577 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o150)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4223 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3577), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtRExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3577), func4229(listloop_r3577));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4223, o150, listloop_r3577)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4234 = () =>
            {
                var smplCommandRemember4235 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4233 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4233.SetZero(smpl);

                foreach (IVariable listloop_r4232 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4233.InjectAdd(smpl, temp4233, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r4232), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r4232));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4235;
                return temp4233;

            };


            O.Assignment o151 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4231 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRExciseDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4234());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtRExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4231, o151, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4242 = (IVariable listloop_i3578) =>
            {
                var smplCommandRemember4243 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4241 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4241.SetZero(smpl);

                foreach (IVariable listloop_s4237 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4241.InjectAdd(smpl, temp4241, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_I_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237)), O.Add(smpl, i4238, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4239, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237)), O.Add(smpl, i4240, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578, listloop_s4237), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578, listloop_s4237)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4243;
                return temp4241;

            };


            O.Assignment o152 = new O.Assignment();
            foreach (IVariable listloop_i3578 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o152)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4236 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3578), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtIExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3578), func4242(listloop_i3578));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4236, o152, listloop_i3578)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4247 = () =>
            {
                var smplCommandRemember4248 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4246 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4246.SetZero(smpl);

                foreach (IVariable listloop_i4245 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4246.InjectAdd(smpl, temp4246, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4245), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4245));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4248;
                return temp4246;

            };


            O.Assignment o153 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4244 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtIExciseDuty_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4247());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtIExciseDuty", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4244, o153, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o154 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4249 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtGExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtXExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtIExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtRExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtExciseDuty", null, ivTmpvar4249, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o154)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4256 = () =>
            {
                var smplCommandRemember4257 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4255 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4255.SetZero(smpl);

                foreach (IVariable listloop_s4251 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4255.InjectAdd(smpl, temp4255, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tRegC", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("ccar"), listloop_s4251), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("ccar"), listloop_s4251)), O.Add(smpl, i4252, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("ccar"), listloop_s4251), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_y", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("ccar"), listloop_s4251))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4253, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("ccar"), listloop_s4251), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("ccar"), listloop_s4251)), O.Lookup(smpl, null, null, "tRegC", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("ccar"), listloop_s4251), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("ccar"), listloop_s4251)), O.Add(smpl, i4254, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("ccar"), listloop_s4251), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("ccar"), listloop_s4251)))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4257;
                return temp4255;

            };


            O.Assignment o155 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4250 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRegC", null, null, new LookupSettings(), EVariableType.Var, null), func4256());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtRegC", null, ivTmpvar4250, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o155)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4264 = () =>
            {
                var smplCommandRemember4265 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4263 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4263.SetZero(smpl);

                foreach (IVariable listloop_g4259 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4263.InjectAdd(smpl, temp4263, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tRegG", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4259, new ScalarString("ser")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4259, new ScalarString("ser"))), O.Add(smpl, i4260, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4259, new ScalarString("ser")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4259, new ScalarString("ser")))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4261, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4259, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4259, new ScalarString("goo"))), O.Lookup(smpl, null, null, "tRegG", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4259, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4259, new ScalarString("goo"))), O.Add(smpl, i4262, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4259, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4259, new ScalarString("goo"))))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4265;
                return temp4263;

            };


            O.Assignment o156 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4258 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRegG", null, null, new LookupSettings(), EVariableType.Var, null), func4264());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtRegG", null, ivTmpvar4258, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o156)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4272 = () =>
            {
                var smplCommandRemember4273 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4271 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4271.SetZero(smpl);

                foreach (IVariable listloop_i4267 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4271.InjectAdd(smpl, temp4271, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tRegI", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4267, new ScalarString("ser")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4267, new ScalarString("ser"))), O.Add(smpl, i4268, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4267, new ScalarString("ser")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4267, new ScalarString("ser")))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Add(smpl, i4269, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4267, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4267, new ScalarString("goo"))), O.Lookup(smpl, null, null, "tRegI", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4267, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4267, new ScalarString("goo"))), O.Add(smpl, i4270, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4267, new ScalarString("goo")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4267, new ScalarString("goo"))))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4273;
                return temp4271;

            };


            O.Assignment o157 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4266 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtRegI", null, null, new LookupSettings(), EVariableType.Var, null), func4272());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtRegI", null, ivTmpvar4266, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o157)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o158 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4274 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtProductSub", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtRegC", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtRegG", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtRegI", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtExciseDuty", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtDuty", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtProductSub", null, ivTmpvar4274, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o158)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o159 = new O.Assignment();
            foreach (IVariable listloop_x3579 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o159)))
            {
                foreach (IVariable listloop_s3580 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o159)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4275 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3579, listloop_s3580), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tDuty_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3579, listloop_s3580), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3579, listloop_s3580), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3579, listloop_s3580)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3579, listloop_s3580), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tProductSub_X_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3579, listloop_s3580));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tDuty_X_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4275, o159, listloop_x3579, listloop_s3580)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o160 = new O.Assignment();
            foreach (IVariable listloop_x3581 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o160)))
            {
                foreach (IVariable listloop_s3582 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o160)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4276 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3581, listloop_s3582), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tDuty_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3581, listloop_s3582), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3581, listloop_s3582), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3581, listloop_s3582)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3581, listloop_s3582), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tProductSub_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3581, listloop_s3582));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tDuty_X_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4276, o160, listloop_x3581, listloop_s3582)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o161 = new O.Assignment();
            foreach (IVariable listloop_r3583 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o161)))
            {
                foreach (IVariable listloop_s3584 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o161)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4277 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3583, listloop_s3584), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tDuty_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3583, listloop_s3584), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3583, listloop_s3584), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3583, listloop_s3584)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3583, listloop_s3584), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tProductSub_R_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3583, listloop_s3584));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tDuty_R_y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4277, o161, listloop_r3583, listloop_s3584)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o162 = new O.Assignment();
            foreach (IVariable listloop_r3585 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o162)))
            {
                foreach (IVariable listloop_s3586 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o162)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4278 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3585, listloop_s3586), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tDuty_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3585, listloop_s3586), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3585, listloop_s3586), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tExciseDuty_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3585, listloop_s3586)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3585, listloop_s3586), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tProductSub_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3585, listloop_s3586));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tDuty_R_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4278, o162, listloop_r3585, listloop_s3586)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4283 = (IVariable listloop_r3587) =>
            {
                var smplCommandRemember4284 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4282 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4282.SetZero(smpl);

                foreach (IVariable listloop_s4280 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4282.InjectAdd(smpl, temp4282, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3587, listloop_s4280), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3587, listloop_s4280), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3587, listloop_s4280), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3587, listloop_s4280)), O.Add(smpl, i4281, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3587, listloop_s4280), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3587, listloop_s4280))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4284;
                return temp4282;

            };


            O.Assignment o163 = new O.Assignment();
            foreach (IVariable listloop_r3587 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, o163)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4279 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r3587), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r3587), func4283(listloop_r3587));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_R_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4279, o163, listloop_r3587)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4288 = () =>
            {
                var smplCommandRemember4289 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4287 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4287.SetZero(smpl);

                foreach (IVariable listloop_r4286 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4287.InjectAdd(smpl, temp4287, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r4286), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r4286));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4289;
                return temp4287;

            };


            O.Assignment o164 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4285 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus_R_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4288());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_R_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4285, o164, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4294 = (IVariable listloop_i3588) =>
            {
                var smplCommandRemember4295 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4293 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4293.SetZero(smpl);

                foreach (IVariable listloop_s4291 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4293.InjectAdd(smpl, temp4293, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3588, listloop_s4291), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3588, listloop_s4291), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3588, listloop_s4291), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3588, listloop_s4291)), O.Add(smpl, i4292, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3588, listloop_s4291), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3588, listloop_s4291))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4295;
                return temp4293;

            };


            O.Assignment o165 = new O.Assignment();
            foreach (IVariable listloop_i3588 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o165)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4290 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3588), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3588), func4294(listloop_i3588));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_I_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4290, o165, listloop_i3588)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4299 = () =>
            {
                var smplCommandRemember4300 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4298 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4298.SetZero(smpl);

                foreach (IVariable listloop_i4297 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4298.InjectAdd(smpl, temp4298, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i4297), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i4297));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4300;
                return temp4298;

            };


            O.Assignment o166 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4296 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus_I_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4299());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_I_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4296, o166, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4305 = (IVariable listloop_g3589) =>
            {
                var smplCommandRemember4306 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4304 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4304.SetZero(smpl);

                foreach (IVariable listloop_s4302 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4304.InjectAdd(smpl, temp4304, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3589, listloop_s4302), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3589, listloop_s4302), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3589, listloop_s4302), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3589, listloop_s4302)), O.Add(smpl, i4303, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3589, listloop_s4302), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3589, listloop_s4302))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4306;
                return temp4304;

            };


            O.Assignment o167 = new O.Assignment();
            foreach (IVariable listloop_g3589 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, o167)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4301 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g3589), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g3589), func4305(listloop_g3589));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_G_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4301, o167, listloop_g3589)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4310 = () =>
            {
                var smplCommandRemember4311 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4309 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4309.SetZero(smpl);

                foreach (IVariable listloop_g4308 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4309.InjectAdd(smpl, temp4309, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g4308), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g4308));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4311;
                return temp4309;

            };


            O.Assignment o168 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4307 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus_G_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4310());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_G_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4307, o168, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4316 = (IVariable listloop_c3590) =>
            {
                var smplCommandRemember4317 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4315 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4315.SetZero(smpl);

                foreach (IVariable listloop_s4313 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4315.InjectAdd(smpl, temp4315, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3590, listloop_s4313), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3590, listloop_s4313), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3590, listloop_s4313), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3590, listloop_s4313)), O.Add(smpl, i4314, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3590, listloop_s4313), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3590, listloop_s4313))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4317;
                return temp4315;

            };


            O.Assignment o169 = new O.Assignment();
            foreach (IVariable listloop_c3590 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o169)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4312 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3590), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3590), func4316(listloop_c3590));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_C_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4312, o169, listloop_c3590)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4321 = () =>
            {
                var smplCommandRemember4322 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4320 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4320.SetZero(smpl);

                foreach (IVariable listloop_c4319 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4320.InjectAdd(smpl, temp4320, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c4319), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c4319));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4322;
                return temp4320;

            };


            O.Assignment o170 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4318 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus_C_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4321());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_C_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4318, o170, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4327 = (IVariable listloop_x3591) =>
            {
                var smplCommandRemember4328 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4326 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4326.SetZero(smpl);

                foreach (IVariable listloop_s4324 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4326.InjectAdd(smpl, temp4326, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3591, listloop_s4324), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3591, listloop_s4324), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3591, listloop_s4324), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3591, listloop_s4324)), O.Add(smpl, i4325, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3591, listloop_s4324), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3591, listloop_s4324))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4328;
                return temp4326;

            };


            O.Assignment o171 = new O.Assignment();
            foreach (IVariable listloop_x3591 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o171)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4323 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3591), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3591), func4327(listloop_x3591));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_X_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4323, o171, listloop_x3591)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4332 = () =>
            {
                var smplCommandRemember4333 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4331 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4331.SetZero(smpl);

                foreach (IVariable listloop_x4330 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4331.InjectAdd(smpl, temp4331, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x4330), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x4330));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4333;
                return temp4331;

            };


            O.Assignment o172 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4329 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus_X_m_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4332());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCus_X_m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4329, o172, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o173 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4334 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCus", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_C_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_G_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_R_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_I_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCus_X_m", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtCus", null, ivTmpvar4334, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o173)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o174 = new O.Assignment();
            foreach (IVariable listloop_x3592 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, o174)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4335 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4336)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592), O.Negate(smpl, i4336)
                ), O.Lookup(smpl, null, null, "Xrigidity", null, null, new LookupSettings(), EVariableType.Var, null))), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Subtract(smpl, i4337, O.Lookup(smpl, null, null, "Xrigidity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qMF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pXF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x3592), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eXF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x3592))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4335, o174, listloop_x3592)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o175 = new O.Assignment();
            foreach (IVariable listloop_c3593 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, o175)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4338 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3593), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qCTourist", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3593), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c3593), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sCTourist", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c3593), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("XTOU")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("XTOU"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qCTourist", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4338, o175, listloop_c3593)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o176 = new O.Assignment();
            foreach (IVariable listloop_sp3594 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o176)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4339 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3594), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qYgross", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3594), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3594), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3594)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3594), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3594)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3594), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKapUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3594)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3594), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qLabUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3594));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qYgross", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4339, o176, listloop_sp3594)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o177 = new O.Assignment();
            foreach (IVariable listloop_k3595 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o177)))
            {
                foreach (IVariable listloop_sp3596 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o177)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4340 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3595, listloop_sp3596), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_rK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3595, listloop_sp3596), O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "rRiskPrem", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3595, listloop_sp3596), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rKprem", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3595, listloop_sp3596));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "rK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4340, o177, listloop_k3595, listloop_sp3596)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o178 = new O.Assignment();
            foreach (IVariable listloop_k3597 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o178)))
            {
                foreach (IVariable listloop_sp3598 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o178)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4341 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3597, listloop_sp3598), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3597, listloop_sp3598), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3597), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3597), i4342), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4343)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3597, listloop_sp3598), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3597, listloop_sp3598), O.Negate(smpl, i4343)
                    )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), Functions.power(smpl, O.Subtract(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3597, listloop_sp3598), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3597, listloop_sp3598), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4344)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3597, listloop_sp3598), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3597, listloop_sp3598), O.Negate(smpl, i4344)
                    )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "fKInstCost", null, null, new LookupSettings(), EVariableType.Var, null)), i4345)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qKInstCost", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4341, o178, listloop_k3597, listloop_sp3598)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4349 = (IVariable listloop_sp3599) =>
            {
                var smplCommandRemember4350 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4348 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4348.SetZero(smpl);

                foreach (IVariable listloop_k4347 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4348.InjectAdd(smpl, temp4348, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4347, listloop_sp3599), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4347, listloop_sp3599));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4350;
                return temp4348;

            };


            O.Assignment o179 = new O.Assignment();
            foreach (IVariable listloop_sp3599 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o179)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4346 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3599), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qKInstCost_kTot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3599), func4349(listloop_sp3599));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qKInstCost", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4346, o179, new ScalarString("tot"), listloop_sp3599)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o180 = new O.Assignment();
            foreach (IVariable listloop_rp3600 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("rp")))), null, new LookupSettings(), EVariableType.Var, o180)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4351 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sYR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qYgross", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fAgeK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_rp3600), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_rp3600))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4351, o180, listloop_rp3600)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o181 = new O.Assignment();
            foreach (IVariable listloop_sp3601 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o181)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4352 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3601), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3601), O.Divide(smpl, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4353)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3601), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3601), O.Negate(smpl, i4353)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4354)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3601), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3601), O.Negate(smpl, i4354)
                )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3601), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3601)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3601), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3601)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4352, o181, listloop_sp3601)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o182 = new O.Assignment();
            foreach (IVariable listloop_sp3602 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o182)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4355 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sYKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qYgross", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fAgeK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602)), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3602), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3602))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4355, o182, listloop_sp3602)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o183 = new O.Assignment();
            foreach (IVariable listloop_k3603 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o183)))
            {
                foreach (IVariable listloop_sp3604 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o183)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4356 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3603, listloop_sp3604), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_fLabUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3603, listloop_sp3604), O.Multiply(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3603, listloop_sp3604), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "thetakul", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3603, listloop_sp3604), O.Add(smpl, i4357, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null))), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3603, listloop_sp3604), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3603, listloop_sp3604), O.Add(smpl, i4358, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3604), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "LabUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3604), O.Add(smpl, i4359, O.Lookup(smpl, null, null, "eLabUtil", null, null, new LookupSettings(), EVariableType.Var, null)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "fLabUtilCost", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4356, o183, listloop_k3603, listloop_sp3604)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o184 = new O.Assignment();
            foreach (IVariable listloop_k3605 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o184)))
            {
                foreach (IVariable listloop_sp3606 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o184)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4360 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3605, listloop_sp3606), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_fKapUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3605, listloop_sp3606), O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3605, listloop_sp3606), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "thetakuk", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3605, listloop_sp3606), O.Add(smpl, i4361, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null))), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3605, listloop_sp3606), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3605, listloop_sp3606), O.Add(smpl, i4362, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "fKapUtilCost", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4360, o184, listloop_k3605, listloop_sp3606)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o185 = new O.Assignment();
            foreach (IVariable listloop_k3607 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o185)))
            {
                foreach (IVariable listloop_sp3608 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o185)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4363 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3607, listloop_sp3608), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_dLabUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3607, listloop_sp3608), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3608), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3608), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3607, listloop_sp3608), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "thetakul", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3607, listloop_sp3608)), O.Add(smpl, i4364, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null))), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3607, listloop_sp3608), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3607, listloop_sp3608), O.Add(smpl, i4365, O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3608), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "LabUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3608), O.Lookup(smpl, null, null, "eLabUtil", null, null, new LookupSettings(), EVariableType.Var, null))), O.Add(smpl, i4366, O.Lookup(smpl, null, null, "eLabUtil", null, null, new LookupSettings(), EVariableType.Var, null))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "dLabUtil", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4363, o185, listloop_k3607, listloop_sp3608)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o186 = new O.Assignment();
            foreach (IVariable listloop_k3609 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o186)))
            {
                foreach (IVariable listloop_sp3610 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o186)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4367 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3609, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_dKapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3609, listloop_sp3610), O.Multiply(smpl, O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3609, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3609, listloop_sp3610), O.Lookup(smpl, null, null, "eKapUtil", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3610), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3609, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "thetakul", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3609, listloop_sp3610)), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "LabUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3610), O.Add(smpl, i4368, O.Lookup(smpl, null, null, "eLabUtil", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4369)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3609, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3609, listloop_sp3610), O.Negate(smpl, i4369)
                    ), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3609, listloop_sp3610), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "thetakuk", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3609, listloop_sp3610)))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "dKapUtil", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4367, o186, listloop_k3609, listloop_sp3610)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o187 = new O.Assignment();
            foreach (IVariable listloop_sp3611 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o187)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4370 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pLuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3611), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3611)), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3611), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qLabUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3611)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3611)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pLuser", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4370, o187, listloop_sp3611)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o188 = new O.Assignment();
            foreach (IVariable listloop_sp3612 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o188)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4371 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3612), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3612), O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4371, o188, listloop_sp3612)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o189 = new O.Assignment();
            foreach (IVariable listloop_sp3613 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o189)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4372 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3613), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3613), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3613), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3613), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3613), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3613)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4372, o189, listloop_sp3613)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o190 = new O.Assignment();
            foreach (IVariable listloop_sp3614 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o190)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4373 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK_kTot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3614), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3614), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4374
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3614), i4374
                )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Power(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4375
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3614), i4375
                ), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3614)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3614), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eKL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3614))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4373, o190, new ScalarString("tot"), listloop_sp3614)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o191 = new O.Assignment();
            foreach (IVariable listloop_sp3615 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o191)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4376 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3615), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK_kTot_end", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3615), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4377)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3615), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3615), O.Negate(smpl, i4377)
                ));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4376, o191, new ScalarString("tot"), listloop_sp3615)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o192 = new O.Assignment();
            foreach (IVariable listloop_k3616 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o192)))
            {
                foreach (IVariable listloop_sp3617 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o192)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4378 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3616, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3616, listloop_sp3617), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4379)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3616, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3616, listloop_sp3617), O.Negate(smpl, i4379)
                    ), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4380)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3616, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3616, listloop_sp3617), O.Negate(smpl, i4380)
                    )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3616, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3616, listloop_sp3617)), O.Add(smpl, i4381, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3617))), O.Subtract(smpl, i4382, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3617))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "dKapUtil", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4378, o192, listloop_k3616, listloop_sp3617)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o193 = new O.Assignment();
            foreach (IVariable listloop_k3618 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o193)))
            {
                foreach (IVariable listloop_sp3619 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o193)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4383 = O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tobinsQ", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), O.Add(smpl, i4384, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4385
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4385
                    )))), O.Multiply(smpl, O.Multiply(smpl, O.Subtract(smpl, i4386, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4387
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4387
                    )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4388
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tobinsQ", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4388
                    )), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null))), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Divide(smpl, O.Subtract(smpl, i4389, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619)), O.Add(smpl, i4390, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4391
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619), i4391
                    )), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4392
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fKapUtilCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4392
                    ))), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4393
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619), i4393
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4394
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4394
                    )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619))), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Divide(smpl, O.Subtract(smpl, i4395, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619)), O.Add(smpl, i4396, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4397
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3619), i4397
                    )), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618)), i4398), O.Subtract(smpl, Functions.power(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4399
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619), i4399
                    ), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3618, listloop_sp3619), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3618, listloop_sp3619)), i4400), Functions.power(smpl, O.Lookup(smpl, null, null, "fKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), i4401))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4383, o193, listloop_k3618, listloop_sp3619)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4405 = (IVariable listloop_sp3620) =>
            {
                var smplCommandRemember4406 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4404 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4404.SetZero(smpl);

                foreach (IVariable listloop_k4403 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4404.InjectAdd(smpl, temp4404, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4403, listloop_sp3620), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4403, listloop_sp3620), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4403, listloop_sp3620), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4403, listloop_sp3620)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4406;
                return temp4404;

            };


            O.Assignment o194 = new O.Assignment();
            foreach (IVariable listloop_sp3620 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o194)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4402 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3620), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pKuser_tot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3620), O.Divide(smpl, func4405(listloop_sp3620), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3620), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3620)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4402, o194, new ScalarString("tot"), listloop_sp3620)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o195 = new O.Assignment();
            foreach (IVariable listloop_k3621 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o195)))
            {
                foreach (IVariable listloop_sp3622 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o195)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4407 = O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_tobinsQ", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622)), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3622), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKInstCost", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621)), O.Subtract(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4408)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3621, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3621, listloop_sp3622), O.Negate(smpl, i4408)
                    )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "fKInstCost", null, null, new LookupSettings(), EVariableType.Var, null))), O.Subtract(smpl, i4409, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3622))), O.Add(smpl, i4410, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3622), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3622))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "tobinsQ", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4407, o195, listloop_k3621, listloop_sp3622)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o196 = new O.Assignment();
            foreach (IVariable listloop_k3623 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o196)))
            {
                foreach (IVariable listloop_sp3624 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o196)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4411 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3623, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_fKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3623, listloop_sp3624), O.Divide(smpl, O.Add(smpl, O.Multiply(smpl, O.Subtract(smpl, i4412, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4413
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3623, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rTaxDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3623, listloop_sp3624), i4413
                    )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4414
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3623, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3623, listloop_sp3624), i4414
                    )), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4415
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3624), i4415
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4416
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3623, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rTaxDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3623, listloop_sp3624), i4416
                    ))), O.Add(smpl, i4417, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4418
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3623, listloop_sp3624), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3623, listloop_sp3624), i4418
                    ))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "fKtax", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4411, o196, listloop_k3623, listloop_sp3624)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o197 = new O.Assignment();
            foreach (IVariable listloop_k3625 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o197)))
            {
                foreach (IVariable listloop_sp3626 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o197)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4419 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3625, listloop_sp3626), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_fKtax_end", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3625, listloop_sp3626), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4420)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3625, listloop_sp3626), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3625, listloop_sp3626), O.Negate(smpl, i4420)
                    ));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "fKtax", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4419, o197, listloop_k3625, listloop_sp3626)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o198 = new O.Assignment();
            foreach (IVariable listloop_k3627 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o198)))
            {
                foreach (IVariable listloop_sp3628 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o198)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4421 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3627, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3627, listloop_sp3628), O.Multiply(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3627, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3627, listloop_sp3628), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4422
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3627, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3627, listloop_sp3628), i4422
                    )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3628)), O.Power(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4423
                    ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3627, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "KapUtil", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3627, listloop_sp3628), i4423
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3628)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3627, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3627, listloop_sp3628)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3628), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eKK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3628))));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4421, o198, listloop_k3627, listloop_sp3628)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o199 = new O.Assignment();
            foreach (IVariable listloop_k3629 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o199)))
            {
                foreach (IVariable listloop_sp3630 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o199)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4424 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3629, listloop_sp3630), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK_end", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3629, listloop_sp3630), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4425)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3629, listloop_sp3630), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3629, listloop_sp3630), O.Negate(smpl, i4425)
                    ));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4424, o199, listloop_k3629, listloop_sp3630)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o200 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4426 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qR_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sYR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4426, o200, new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o201 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4427 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pKL_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4428)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub")), O.Negate(smpl, i4428)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4429)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub")), O.Negate(smpl, i4429)
            )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4427, o201, new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o202 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4430 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qKL_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sYKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4430, o202, new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o203 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4431 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pL_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4431, o203, new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o204 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4432 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vL_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4432, o204, new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o205 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4433 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qK_kTot_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4434
            ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), i4434
            )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)), O.Power(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4435
            ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")), i4435
            ), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eKL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4433, o205, new ScalarString("tot"), new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o206 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4436 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qK_kTot_end_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4437)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub")), O.Negate(smpl, i4437)
            ));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4436, o206, new ScalarString("tot"), new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o207 = new O.Assignment();
            foreach (IVariable listloop_k3631 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o207)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4438 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3631), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pKuser_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3631), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3631), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fpKuser_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3631), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4439
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3631, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3631, new ScalarString("pub")), i4439
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4440
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3631, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3631, new ScalarString("pub")), i4440
                )), O.Lookup(smpl, null, null, "fp", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4438, o207, listloop_k3631, new ScalarString("pub"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4444 = () =>
            {
                var smplCommandRemember4445 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4443 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4443.SetZero(smpl);

                foreach (IVariable listloop_k4442 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4443.InjectAdd(smpl, temp4443, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4442, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4442, new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4442, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4442, new ScalarString("pub"))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4445;
                return temp4443;

            };


            O.Assignment o208 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4441 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pKuser_tot_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func4444(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4441, o208, new ScalarString("tot"), new ScalarString("pub"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o209 = new O.Assignment();
            foreach (IVariable listloop_k3632 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o209)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4446 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3632), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3632), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3632, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sKK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3632, new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub"))), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("pub")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3632, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pKuser", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3632, new ScalarString("pub"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eKK", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("pub")))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4446, o209, listloop_k3632, new ScalarString("pub"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o210 = new O.Assignment();
            foreach (IVariable listloop_k3633 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o210)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4447 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3633), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qK_end_pub", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3633), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4448)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3633, new ScalarString("pub")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3633, new ScalarString("pub")), O.Negate(smpl, i4448)
                ));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4447, o210, listloop_k3633, new ScalarString("pub"))
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o211 = new O.Assignment();
            foreach (IVariable listloop_k3634 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o211)))
            {
                foreach (IVariable listloop_s3635 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o211)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4449 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3634, listloop_s3635), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3634, listloop_s3635), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3634, listloop_s3635), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3634, listloop_s3635)), O.Divide(smpl, O.Multiply(smpl, O.Subtract(smpl, i4450, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3634, listloop_s3635), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3634, listloop_s3635)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4451)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3634, listloop_s3635), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3634, listloop_s3635), O.Negate(smpl, i4451)
                    )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null)));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4449, o211, listloop_k3634, listloop_s3635)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o212 = new O.Assignment();
            foreach (IVariable listloop_s3636 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o212)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4452 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3636), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qI_Inv", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3636), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3636), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sInventory", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3636), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3636), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3636)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4452, o212, new ScalarString("invt"), listloop_s3636)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4456 = (IVariable listloop_s3637) =>
            {
                var smplCommandRemember4457 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4455 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4455.SetZero(smpl);

                foreach (IVariable listloop_ss4454 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("ss")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4455.InjectAdd(smpl, temp4455, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_ss4454), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fnL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_ss4454), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_ss4454), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_ss4454)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4457;
                return temp4455;

            };


            O.Assignment o213 = new O.Assignment();
            foreach (IVariable listloop_s3637 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o213)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4453 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3637), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_nL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3637), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3637), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fnL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3637), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3637), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3637)), func4456(listloop_s3637)), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "nCrossBorder", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4453, o213, listloop_s3637)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4461 = () =>
            {
                var smplCommandRemember4462 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4460 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4460.SetZero(smpl);

                foreach (IVariable listloop_s4459 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4460.InjectAdd(smpl, temp4460, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4459), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4459));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4462;
                return temp4460;

            };


            O.Assignment o214 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4458 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nL_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4461());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4458, o214, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o215 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4463 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pL_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4463, o215, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4467 = () =>
            {
                var smplCommandRemember4468 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4466 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4466.SetZero(smpl);

                foreach (IVariable listloop_s4465 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4466.InjectAdd(smpl, temp4466, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4465), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4465));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4468;
                return temp4466;

            };


            O.Assignment o216 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4464 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qL_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4467());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4464, o216, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4472 = () =>
            {
                var smplCommandRemember4473 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4471 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4471.SetZero(smpl);

                foreach (IVariable listloop_s4470 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4471.InjectAdd(smpl, temp4471, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s4470), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s4470));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4473;
                return temp4471;

            };


            O.Assignment o217 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4469 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vL_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4472());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4469, o217, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o218 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4474 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_rFirms", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "rRiskPrem", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "rFirms", null, ivTmpvar4474, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o218)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o219 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4475 = O.Add(smpl, O.Lookup(smpl, null, null, "J_rEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Divide(smpl, O.Lookup(smpl, null, null, "vrEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4476)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4476)
            )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "rEquity", null, ivTmpvar4475, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o219)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o220 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4477 = O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vrEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vEquity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4478)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4478)
            ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vrEquity", null, ivTmpvar4477, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o220)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o221 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4479 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_rEquityForeign", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "rRiskPrem", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "rEquityForeign", null, ivTmpvar4479, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o221)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o222 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4480 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Add(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4481
            ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), i4481
            ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4482
            ), smpl, O.EIndexerType.IndexerLead, O.Lookup(smpl, null, null, "vEquity", null, null, new LookupSettings(), EVariableType.Var, null), i4482
            ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null))), O.Add(smpl, i4483, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4484
            ), smpl, O.EIndexerType.IndexerLead, O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null), i4484
            ))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vEquity", null, ivTmpvar4480, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o222)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o223 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4485 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vEquity_end", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, O.Subtract(smpl, i4486, O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vEquity", null, ivTmpvar4485, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o223)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o224 = new O.Assignment();
            foreach (IVariable listloop_sp3638 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o224)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4487 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3638), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vrFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3638), O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4488)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3638), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3638), O.Negate(smpl, i4488)
                )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vrFirmDebt", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4487, o224, listloop_sp3638)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4492 = () =>
            {
                var smplCommandRemember4493 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4491 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4491.SetZero(smpl);

                foreach (IVariable listloop_sp4490 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4491.InjectAdd(smpl, temp4491, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp4490), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp4490));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4493;
                return temp4491;

            };


            O.Assignment o225 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4489 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vrFirmDebt_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4492());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vrFirmDebt", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4489, o225, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o226 = new O.Assignment();
            foreach (IVariable listloop_sp3639 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o226)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4494 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3639), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_rWACC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3639), O.Multiply(smpl, O.Lookup(smpl, null, null, "rEquity", null, null, new LookupSettings(), EVariableType.Var, null), O.Subtract(smpl, i4495, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3639), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3639)))), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3639), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3639)), O.Subtract(smpl, i4496, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3639), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3639))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "rWACC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4494, o226, listloop_sp3639)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o227 = new O.Assignment();
            foreach (IVariable listloop_sp3640 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o227)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4497 = O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3640), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vEBITDA", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3640), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3640), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3640), O.Add(smpl, i4498, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3640), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3640)))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3640), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vL", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3640)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3640), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3640));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEBITDA", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4497, o227, listloop_sp3640)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4503 = (IVariable listloop_sp3641) =>
            {
                var smplCommandRemember4504 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4502 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4502.SetZero(smpl);

                foreach (IVariable listloop_k4500 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4502.InjectAdd(smpl, temp4502, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4500, listloop_sp3641), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rTaxDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4500, listloop_sp3641), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4501)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4500, listloop_sp3641), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4500, listloop_sp3641), O.Negate(smpl, i4501)
                    )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4504;
                return temp4502;

            };


            O.Assignment o228 = new O.Assignment();
            foreach (IVariable listloop_sp3641 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o228)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4499 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3641), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vEBIT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3641), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3641), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEBITDA", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3641)), func4503(listloop_sp3641));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEBIT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4499, o228, listloop_sp3641)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o229 = new O.Assignment();
            foreach (IVariable listloop_sp3642 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o229)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4505 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3642), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vEBT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3642), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3642), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEBIT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3642)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3642), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3642));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEBT", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4505, o229, listloop_sp3642)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4511 = (IVariable listloop_sp3643) =>
            {
                var smplCommandRemember4512 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4510 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4510.SetZero(smpl);

                foreach (IVariable listloop_k4508 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4510.InjectAdd(smpl, temp4510, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4508, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rTaxDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4508, listloop_sp3643), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4509)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4508, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4508, listloop_sp3643), O.Negate(smpl, i4509)
                    )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4512;
                return temp4510;

            };


            O.Assignment o230 = new O.Assignment();
            foreach (IVariable listloop_sp3643 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o230)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4506 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vFCFE", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3643), O.Multiply(smpl, O.Subtract(smpl, i4507, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3643)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEBT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3643))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), listloop_sp3643)), func4511(listloop_sp3643)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3643)), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4513)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3643), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3643), O.Negate(smpl, i4513)
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4506, o230, listloop_sp3643)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4517 = () =>
            {
                var smplCommandRemember4518 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4516 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4516.SetZero(smpl);

                foreach (IVariable listloop_sp4515 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4516.InjectAdd(smpl, temp4516, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp4515), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp4515));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4518;
                return temp4516;

            };


            O.Assignment o231 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4514 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vFCFE_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4517());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vFCFE", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4514, o231, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o232 = new O.Assignment();
            foreach (IVariable listloop_k3644 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, o232)))
            {
                foreach (IVariable listloop_sp3645 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o232)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar4519 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3644, listloop_sp3645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3644, listloop_sp3645), O.Divide(smpl, O.Multiply(smpl, O.Subtract(smpl, i4520, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3644, listloop_sp3645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rTaxDepr", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3644, listloop_sp3645)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4521)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3644, listloop_sp3645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3644, listloop_sp3645), O.Negate(smpl, i4521)
                    )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k3644, listloop_sp3645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k3644, listloop_sp3645));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4519, o232, listloop_k3644, listloop_sp3645)
                    ;
                }
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4525 = (IVariable listloop_sp3646) =>
            {
                var smplCommandRemember4526 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4524 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4524.SetZero(smpl);

                foreach (IVariable listloop_k4523 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4524.InjectAdd(smpl, temp4524, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4523, listloop_sp3646), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4523, listloop_sp3646));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4526;
                return temp4524;

            };


            O.Assignment o233 = new O.Assignment();
            foreach (IVariable listloop_sp3646 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o233)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4522 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3646), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vKtax_tot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3646), func4525(listloop_sp3646));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vKtax", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4522, o233, new ScalarString("tot"), listloop_sp3646)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4530 = (IVariable listloop_sp3647) =>
            {
                var smplCommandRemember4531 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4529 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4529.SetZero(smpl);

                foreach (IVariable listloop_k4528 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4529.InjectAdd(smpl, temp4529, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4528, listloop_sp3647), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4528, listloop_sp3647), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_k4528, listloop_sp3647), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qK", null, null, new LookupSettings(), EVariableType.Var, null), listloop_k4528, listloop_sp3647)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4531;
                return temp4529;

            };


            O.Assignment o234 = new O.Assignment();
            foreach (IVariable listloop_sp3647 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o234)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4527 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3647), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3647), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3647), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3647), func4530(listloop_sp3647)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4527, o234, listloop_sp3647)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4535 = () =>
            {
                var smplCommandRemember4536 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4534 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4534.SetZero(smpl);

                foreach (IVariable listloop_sp4533 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4534.InjectAdd(smpl, temp4534, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp4533), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp4533));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4536;
                return temp4534;

            };


            O.Assignment o235 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4532 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vFirmDebt_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4535());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4532, o235, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o236 = new O.Assignment();
            foreach (IVariable listloop_sp3648 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o236)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4537 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3648), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pYs", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3648), O.Divide(smpl, O.Multiply(smpl, O.Add(smpl, i4538, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3648), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "calvo_markup", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3648)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3648), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "numerP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3648)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3648), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "denomP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3648)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pYs", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4537, o236, listloop_sp3648)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o237 = new O.Assignment();
            foreach (IVariable listloop_sp3649 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o237)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4539 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_numerP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649)), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eYY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649)))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4540
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3649), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "numerP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3649), i4540
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "pYrigidity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, i4541, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4542
                ), smpl, O.EIndexerType.IndexerLead, O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null), i4542
                ))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "numerP", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4539, o237, listloop_sp3649)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o238 = new O.Assignment();
            foreach (IVariable listloop_sp3650 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o238)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4543 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3650), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_denomP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3650), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3650), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3650), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3650), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3650), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3650), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eYY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3650)))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4544
                ), smpl, O.EIndexerType.IndexerLead, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3650), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "denomP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3650), i4544
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "pYrigidity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, i4545, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLead, i4546
                ), smpl, O.EIndexerType.IndexerLead, O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null), i4546
                ))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "denomP", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4543, o238, listloop_sp3650)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o239 = new O.Assignment();
            foreach (IVariable listloop_sp3651 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o239)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4547 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_numerP_end", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651)), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eYY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651)))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3651), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "numerP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3651), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "pYrigidity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, i4548, O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "numerP", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4547, o239, listloop_sp3651)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o240 = new O.Assignment();
            foreach (IVariable listloop_sp3652 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o240)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4549 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3652), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_denomP_end", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3652), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3652), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3652), O.Power(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3652), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3652), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3652), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eYY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3652)))), O.Divide(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3652), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "denomP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3652), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "pYrigidity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Add(smpl, i4550, O.Lookup(smpl, null, null, "rFirms", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "denomP", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4549, o240, listloop_sp3652)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o241 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4551 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pY_pub", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Add(smpl, i4552, O.Lookup(smpl, null, null, "markup_pub", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("PUB")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY0", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("PUB"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4551, o241, new ScalarString("PUB"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4556 = () =>
            {
                var smplCommandRemember4557 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4555 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4555.SetZero(smpl);

                foreach (IVariable listloop_a4554 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4555.InjectAdd(smpl, temp4555, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4554), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4554));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4557;
                return temp4555;

            };


            O.Assignment o242 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4553 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nLaborForce_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4556());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4553, o242, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o243 = new O.Assignment();
            foreach (IVariable listloop_a3653 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o243)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4558 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3653), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_nNonRetired", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3653), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3653), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3653)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3653), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nRetired", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3653));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nNonRetired", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4558, o243, listloop_a3653)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4562 = () =>
            {
                var smplCommandRemember4563 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4561 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4561.SetZero(smpl);

                foreach (IVariable listloop_a4560 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4561.InjectAdd(smpl, temp4561, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4560), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nNonRetired", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4560));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4563;
                return temp4561;

            };


            O.Assignment o244 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4559 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nNonRetired_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4562());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nNonRetired", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4559, o244, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o245 = new O.Assignment();
            foreach (IVariable listloop_a3654 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o245)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4564 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3654), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_nOutsideLF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3654), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3654), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nNonRetired", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3654)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3654), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3654));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nOutsideLF", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4564, o245, listloop_a3654)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4568 = () =>
            {
                var smplCommandRemember4569 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4567 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4567.SetZero(smpl);

                foreach (IVariable listloop_a4566 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4567.InjectAdd(smpl, temp4567, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4566), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOutsideLF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4566));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4569;
                return temp4567;

            };


            O.Assignment o246 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4565 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nOutsideLF_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4568());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nOutsideLF", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4565, o246, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o247 = new O.Assignment();
            foreach (IVariable listloop_a3655 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o247)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4570 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3655), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3655), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3655), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "u", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3655), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3655), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3655)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4570, o247, listloop_a3655)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4574 = () =>
            {
                var smplCommandRemember4575 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4573 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4573.SetZero(smpl);

                foreach (IVariable listloop_a4572 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4573.InjectAdd(smpl, temp4573, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4572), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4572));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4575;
                return temp4573;

            };


            O.Assignment o248 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4571 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nUnemployed_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4574());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4571, o248, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o249 = new O.Assignment();
            foreach (IVariable listloop_a3656 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o249)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4576 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3656), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3656), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3656), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3656)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3656), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3656));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4576, o249, listloop_a3656)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4580 = () =>
            {
                var smplCommandRemember4581 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4579 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4579.SetZero(smpl);

                foreach (IVariable listloop_a4578 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4579.InjectAdd(smpl, temp4579, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4578), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4578));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4581;
                return temp4579;

            };


            O.Assignment o250 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4577 = O.Add(smpl, O.Lookup(smpl, null, null, "J_nEmployed_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4580());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4577, o250, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o251 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4582 = O.Add(smpl, O.Lookup(smpl, null, null, "J_u_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "u", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4582, o251, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o252 = new O.Assignment();
            foreach (IVariable listloop_a3657 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o252)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4583 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3657), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_sLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3657), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3657), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3657), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3657), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3657)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "sLaborForce", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4583, o252, listloop_a3657)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o253 = new O.Assignment();
            foreach (IVariable listloop_a3658 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o253)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4584 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3658), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_sUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3658), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3658), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3658), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3658), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3658)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "sUnemployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4584, o253, listloop_a3658)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o254 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4585 = O.Add(smpl, O.Lookup(smpl, null, null, "J_sUnemployed_aTot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "sUnemployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4585, o254, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o255 = new O.Assignment();
            foreach (IVariable listloop_a3659 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o255)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4586 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3659), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_sEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3659), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3659), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3659), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3659), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3659)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "sEmployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4586, o255, listloop_a3659)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o256 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4587 = O.Add(smpl, O.Lookup(smpl, null, null, "J_sEmployed_aTot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "sEmployed", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4587, o256, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o257 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4588 = O.Add(smpl, O.Lookup(smpl, null, null, "J_xi", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "prod_a", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "nCrossBorder", null, null, new LookupSettings(), EVariableType.Var, null))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4588, o257, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4592 = () =>
            {
                var smplCommandRemember4593 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4591 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4591.SetZero(smpl);

                foreach (IVariable listloop_a4590 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4591.InjectAdd(smpl, temp4591, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4590), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "prod_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4590), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4590), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4590)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4593;
                return temp4591;

            };


            O.Assignment o258 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4589 = O.Add(smpl, O.Lookup(smpl, null, null, "J_prod_aTot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func4592(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "prod_a", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4589, o258, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o259 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4594 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_gap_u", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "u", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_u", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "gap_u", null, ivTmpvar4594, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o259)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o260 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4595 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_gap_LF", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), i4596);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "gap_LF", null, ivTmpvar4595, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o260)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o261 = new O.Assignment();
            foreach (IVariable listloop_a3660 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o261)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4597 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3660), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_u", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3660), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3660), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_u", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3660), O.Lookup(smpl, null, null, "xi", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "u", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4597, o261, listloop_a3660)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o262 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4598 = O.Add(smpl, O.Lookup(smpl, null, null, "J_w_end", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "rw0", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4599)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4599)
            )));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "w", null, ivTmpvar4598, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o262)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o263 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4600 = O.Add(smpl, O.Lookup(smpl, null, null, "J_w", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4601)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4601)
            ), O.Lookup(smpl, null, null, "rw0", null, null, new LookupSettings(), EVariableType.Var, null)), Functions.exp(smpl, O.Multiply(smpl, O.Negate(smpl, O.Lookup(smpl, null, null, "psi", null, null, new LookupSettings(), EVariableType.Var, null)), O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4602)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "u", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4602)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4603)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_u", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4603)
            ))))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "w", null, ivTmpvar4600, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o263)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o264 = new O.Assignment();
            foreach (IVariable listloop_a3661 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o264)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4604 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3661), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3661), O.Multiply(smpl, O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3661), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "prod_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3661)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4604, o264, listloop_a3661)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4608 = () =>
            {
                var smplCommandRemember4609 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4607 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4607.SetZero(smpl);

                foreach (IVariable listloop_a4606 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4607.InjectAdd(smpl, temp4607, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4606), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4606), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4606), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4606)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4609;
                return temp4607;

            };


            O.Assignment o265 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4605 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vW_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, func4608(), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "nCrossBorder", null, null, new LookupSettings(), EVariableType.Var, null))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4605, o265, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4614 = () =>
            {
                var smplCommandRemember4615 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4613 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4613.SetZero(smpl);

                foreach (IVariable listloop_a4611 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4613.InjectAdd(smpl, temp4613, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4611), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4611)), O.Subtract(smpl, i4612, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4611), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sSelfEmp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4611))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4615;
                return temp4613;

            };


            O.Assignment o266 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4610 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vPayroll", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, func4614(), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "nCrossBorder", null, null, new LookupSettings(), EVariableType.Var, null))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vPayroll", null, ivTmpvar4610, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o266)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4619 = (IVariable listloop_a3662) =>
            {
                var smplCommandRemember4620 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4618 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4618.SetZero(smpl);

                foreach (IVariable listloop_sp4617 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4618.InjectAdd(smpl, temp4618, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp4617), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp4617));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4620;
                return temp4618;

            };

            Func<IVariable, IVariable> func4623 = (IVariable listloop_a3662) =>
            {
                var smplCommandRemember4624 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4622 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4622.SetZero(smpl);

                foreach (IVariable listloop_aa4621 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("aa")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4622.InjectAdd(smpl, temp4622, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aa4621), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aa4621));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4624;
                return temp4622;

            };


            O.Assignment o267 = new O.Assignment();
            foreach (IVariable listloop_a3662 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o267)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4616 = O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vPrimIncome", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTrans", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662)), O.Divide(smpl, O.Lookup(smpl, null, null, "vGovExpRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Divide(smpl, O.Lookup(smpl, null, null, "vGovInv", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Divide(smpl, O.Lookup(smpl, null, null, "vGovSub", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Divide(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Lookup(smpl, null, null, "vGovRev", null, null, new LookupSettings(), EVariableType.Var, null), func4619(listloop_a3662)), O.Lookup(smpl, null, null, "vDutyVAT", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtY", null, null, new LookupSettings(), EVariableType.Var, null)), func4623(listloop_a3662))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3662), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncomeRes", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3662));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPrimIncome", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4616, o267, listloop_a3662)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4628 = () =>
            {
                var smplCommandRemember4629 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4627 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4627.SetZero(smpl);

                foreach (IVariable listloop_a4626 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4627.InjectAdd(smpl, temp4627, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4626), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncome", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4626), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4626), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4626)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4629;
                return temp4627;

            };


            O.Assignment o268 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4625 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vPrimIncome_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4628());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPrimIncome", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4625, o268, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4633 = () =>
            {
                var smplCommandRemember4634 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4632 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4632.SetZero(smpl);

                foreach (IVariable listloop_a4631 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4632.InjectAdd(smpl, temp4632, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4631), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncomeRes", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4631), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4631), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4631)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4634;
                return temp4632;

            };


            O.Assignment o269 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4630 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vPrimIncomeRes_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4633());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPrimIncomeRes", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4630, o269, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o270 = new O.Assignment();
            foreach (IVariable listloop_a3663 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o270)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4635 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3663), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_c_habit_tEnd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3663), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4636)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3663), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3663), O.Negate(smpl, i4636)
                ));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qCR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4635, o270, listloop_a3663)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o271 = new O.Assignment();
            foreach (IVariable listloop_a3664 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o271)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4637 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3664), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qCR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3664), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3664), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "c_habit", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3664)), O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "habit", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4638)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3664), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3664), O.Negate(smpl, i4638)
                )), O.Lookup(smpl, null, null, "fq", null, null, new LookupSettings(), EVariableType.Var, null))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3664), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_qCR_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3664)), O.Lookup(smpl, null, null, "adj_qCR", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qCR", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4637, o271, listloop_a3664)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o272 = new O.Assignment();
            foreach (IVariable listloop_aEnd3665 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("aEnd")))), null, new LookupSettings(), EVariableType.Var, o272)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4639 = O.Add(smpl, O.Lookup(smpl, null, null, "J_c_habit_aEnd", null, null, new LookupSettings(), EVariableType.Var, null), i4640);
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4639, o272, listloop_aEnd3665)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o273 = new O.Assignment();
            foreach (IVariable listloop_a3666 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o273)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4641 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3666), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qCHtM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3666), O.Divide(smpl, O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3666), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDisp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3666), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3666), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3666)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qCHtM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4641, o273, listloop_a3666)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o274 = new O.Assignment();
            foreach (IVariable listloop_a3667 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o274)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4642 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3667), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3667), O.Multiply(smpl, O.Subtract(smpl, i4643, O.Lookup(smpl, null, null, "sHtM", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3667), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3667))), O.Multiply(smpl, O.Lookup(smpl, null, null, "sHtM", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3667), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qCHtM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3667)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC_a", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4642, o274, listloop_a3667)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4647 = () =>
            {
                var smplCommandRemember4648 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4646 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4646.SetZero(smpl);

                foreach (IVariable listloop_a4645 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4646.InjectAdd(smpl, temp4646, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4645), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4645), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4645)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4648;
                return temp4646;

            };


            O.Assignment o275 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4644 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qC_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4647());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4644, o275, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o276 = new O.Assignment();
            foreach (IVariable listloop_a3668 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o276)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4649 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3668), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3668), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3668), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3668)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vC_a", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4649, o276, listloop_a3668)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o277 = new O.Assignment();
            foreach (IVariable listloop_a3669 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o277)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4650 = O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3669), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vWealth", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3669), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4652)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a3669, i4651, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a3669, i4651, true)), O.Negate(smpl, i4652)
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4654)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a3669, i4653, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a3669, i4653, true)), O.Negate(smpl, i4654)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3669), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3669))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3669), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDisp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3669)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3669), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3669)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3669), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vChildTransfer_parent", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3669));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4650, o277, listloop_a3669)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o278 = new O.Assignment();
            foreach (IVariable listloop_a13670 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a1")))), null, new LookupSettings(), EVariableType.Var, o278)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4655 = O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vWealth_a1", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4656)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aChildE", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aChildE", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Negate(smpl, i4656)
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4657)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aChildE", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aChildE", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Negate(smpl, i4657)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a13670), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a13670))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a13670), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDisp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a13670)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a13670), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC_a", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a13670)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a13670), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vChildTransfer_parent", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a13670));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4655, o278, listloop_a13670)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4661 = () =>
            {
                var smplCommandRemember4662 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4660 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4660.SetZero(smpl);

                foreach (IVariable listloop_aALL4659 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("aALL")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4660.InjectAdd(smpl, temp4660, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aALL4659), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aALL4659), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aALL4659), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aALL4659)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4662;
                return temp4660;

            };


            O.Assignment o279 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4658 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vWealth_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4661());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4658, o279, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o280 = new O.Assignment();
            foreach (IVariable listloop_a_3671 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a_")))), null, new LookupSettings(), EVariableType.Var, o280)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4663 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3671), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vWealth_child", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3671), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4665)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a_3671, i4664, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a_3671, i4664, true)), O.Negate(smpl, i4665)
                ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4667)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a_3671, i4666, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a_3671, i4666, true)), O.Negate(smpl, i4667)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3671), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3671))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3671), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3671)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3671), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vChildTransfer_child", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3671));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4663, o280, listloop_a_3671)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o281 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4668 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vWealth_child_a0", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i4670
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vChildTransfer_child", null, null, new LookupSettings(), EVariableType.Var, null), i4670
            ));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4668, o281, i4669
            )
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func4674 = (IVariable listloop_a3672) =>
            {
                var smplCommandRemember4675 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4673 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4673.SetZero(smpl);

                foreach (IVariable listloop_aChild4672 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("aChild")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4673.InjectAdd(smpl, temp4673, O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aChild4672, listloop_a3672), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sParent", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aChild4672, listloop_a3672), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aChild4672), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vChildTransfer_Child", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aChild4672)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aChild4672), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aChild4672)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4675;
                return temp4673;

            };


            O.Assignment o282 = new O.Assignment();
            foreach (IVariable listloop_a3672 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o282)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4671 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3672), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vChildTransfer_parent", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3672), O.Divide(smpl, func4674(listloop_a3672), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3672), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3672)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vChildTransfer_parent", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4671, o282, listloop_a3672)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o283 = new O.Assignment();
            foreach (IVariable listloop_a_3673 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a_")))), null, new LookupSettings(), EVariableType.Var, o283)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4676 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3673), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_rBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3673), O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null)), O.Divide(smpl, O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "bankLending", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4678)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a_3673, i4677, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a_3673, i4677, true)), O.Negate(smpl, i4678)
                )), O.Subtract(smpl, i4679, O.Lookup(smpl, null, null, "sHtM", null, null, new LookupSettings(), EVariableType.Var, null))), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "rBanks", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4676, o283, listloop_a_3673)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o284 = new O.Assignment();
            foreach (IVariable listloop_a_3674 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a_")))), null, new LookupSettings(), EVariableType.Var, o284)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4680 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_rHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674))), O.Multiply(smpl, O.Multiply(smpl, O.Subtract(smpl, i4681, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBonds", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674)), O.Lookup(smpl, null, null, "rRF", null, null, new LookupSettings(), EVariableType.Var, null))), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Subtract(smpl, i4682, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674)), O.Subtract(smpl, i4683, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBonds", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674))), O.Lookup(smpl, null, null, "sForeign", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "rEquityForeign", null, null, new LookupSettings(), EVariableType.Var, null))), O.Multiply(smpl, O.Multiply(smpl, O.Multiply(smpl, O.Subtract(smpl, i4684, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBanks", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674)), O.Subtract(smpl, i4685, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3674), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sBonds", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3674))), O.Subtract(smpl, i4686, O.Lookup(smpl, null, null, "sForeign", null, null, new LookupSettings(), EVariableType.Var, null))), O.Lookup(smpl, null, null, "rEquity", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "rHH", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4680, o284, listloop_a_3674)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o285 = new O.Assignment();
            foreach (IVariable listloop_a_3675 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a_")))), null, new LookupSettings(), EVariableType.Var, o285)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4687 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3675), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vrHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3675), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3675), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "rHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3675), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4689)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a_3675, i4688, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a_3675, i4688, true)), O.Negate(smpl, i4689)
                )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4691)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.AddSpecial(smpl, listloop_a_3675, i4690, true)), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), O.AddSpecial(smpl, listloop_a_3675, i4690, true)), O.Negate(smpl, i4691)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a_3675), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a_3675)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4687, o285, listloop_a_3675)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4695 = () =>
            {
                var smplCommandRemember4696 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4694 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4694.SetZero(smpl);

                foreach (IVariable listloop_aALLx04693 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("aALLx0")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4694.InjectAdd(smpl, temp4694, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aALLx04693), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aALLx04693), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_aALLx04693), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_aALLx04693)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4696;
                return temp4694;

            };


            O.Assignment o286 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4692 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vrHH_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4695());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4692, o286, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o287 = new O.Assignment();
            foreach (IVariable listloop_c_3676 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c_")))), null, new LookupSettings(), EVariableType.Var, o287)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4697 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3676, O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3676, O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3676), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3676), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                )), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c_3676), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c_3676)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eC", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#cu", null, null, new LookupSettings(), EVariableType.Var, null)
                ))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4697, o287, listloop_c_3676)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o288 = new O.Assignment();
            foreach (IVariable listloop_a3677 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o288)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4698 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3677), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vDisp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3677), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3677), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncome", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3677)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3677), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3677)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3677), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDispRes", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3677));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vDisp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4698, o288, listloop_a3677)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4702 = () =>
            {
                var smplCommandRemember4703 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4701 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4701.SetZero(smpl);

                foreach (IVariable listloop_a4700 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4701.InjectAdd(smpl, temp4701, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4700), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDispRes", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4700), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4700), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4700)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4703;
                return temp4701;

            };


            O.Assignment o289 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4699 = O.Add(smpl, O.Lookup(smpl, null, null, "J_DispRes_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4702());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vDispRes", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4699, o289, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o290 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4704 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vDisp_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncome", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDispRes", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vDisp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4704, o290, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o291 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4705 = O.Add(smpl, O.Lookup(smpl, null, null, "J_fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4706)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4706)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4707)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4707)
            )), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4708)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4708)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4709)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4709)
            ))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "fGov", null, ivTmpvar4705, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o291)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o292 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4710 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_qG_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4711)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i4711)
            )), O.Lookup(smpl, null, null, "adj_qG", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4710, o292, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o293 = new O.Assignment();
            foreach (IVariable listloop_g_3678 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g_")))), null, new LookupSettings(), EVariableType.Var, o293)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4712 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3678, O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3678, O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3678), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "sG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3678), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                )), O.Power(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g_3678), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g_3678)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "eG", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#gu", null, null, new LookupSettings(), EVariableType.Var, null)
                ))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4712, o293, listloop_g_3678)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o294 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4713 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovPrimBalance", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovRev", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovExp", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovPrimBalance", null, ivTmpvar4713, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o294)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o295 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4714 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovBalance", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovPrimBalance", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovNetInterest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovBalance", null, ivTmpvar4714, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o295)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o296 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4715 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovNetInterest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovIntOnAssets", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovIntOnDebt", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovNetInterest", null, ivTmpvar4715, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o296)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o297 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4716 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovIntOnAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "iGovAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4717)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vGovAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4717)
            )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovIntOnAssets", null, ivTmpvar4716, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o297)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o298 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4718 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovIntOnDebt", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "iGovDebt", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4719)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vGovDebt", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4719)
            )), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovIntOnDebt", null, ivTmpvar4718, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o298)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o299 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4720 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovDebt", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovAssets", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovDebt", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovWealth", null, ivTmpvar4720, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o299)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o300 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4721 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Divide(smpl, O.Lookup(smpl, null, null, "vGovAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovDebt", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "DebtReaction", null, ivTmpvar4721, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o300)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o301 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4722 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4723)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vGovWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4723)
            ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null))), O.Lookup(smpl, null, null, "vGovBalance", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovReval", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "adj_vGovClose", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovWealth", null, ivTmpvar4722, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o301)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o302 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4724 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vrGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovNetInterest", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovReval", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vrGov", null, ivTmpvar4724, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o302)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o303 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4725 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovClose", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4726)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vGovWealth", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4726)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovWealth", null, ivTmpvar4725, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o303)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o304 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4727 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovRev", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtDirect", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtIndirect", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovRevRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovRev", null, ivTmpvar4727, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o304)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o305 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4728 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtDirect", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtSource", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtHHWeight", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Lookup(smpl, null, null, "vtPAL", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtMedia", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtDirect", null, ivTmpvar4728, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o305)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o306 = new O.Assignment();
            foreach (IVariable listloop_a3679 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o306)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4729 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtSource", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtBot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtTop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtMun", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtChu", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtProp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtStock", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3679), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3679));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtSource", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4729, o306, listloop_a3679)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4733 = () =>
            {
                var smplCommandRemember4734 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4732 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4732.SetZero(smpl);

                foreach (IVariable listloop_a4731 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4732.InjectAdd(smpl, temp4732, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4731), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtSource", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4731), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4731), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4731)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4734;
                return temp4732;

            };


            O.Assignment o307 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4730 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtSourceTot", null, null, new LookupSettings(), EVariableType.Var, null), func4733());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtSource", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4730, o307, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o308 = new O.Assignment();
            foreach (IVariable listloop_a3680 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o308)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4735 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3680), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtBot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3680), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tBot", null, null, new LookupSettings(), EVariableType.Var, null), O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3680), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3680), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3680), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPositiveNetCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3680)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3680), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3680))), O.Add(smpl, i4736, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3680), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "Adj_vtBot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3680))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtBot", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4735, o308, listloop_a3680)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4740 = () =>
            {
                var smplCommandRemember4741 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4739 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4739.SetZero(smpl);

                foreach (IVariable listloop_a4738 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4739.InjectAdd(smpl, temp4739, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4738), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtBot", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4738), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4738), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4738)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4741;
                return temp4739;

            };


            O.Assignment o309 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4737 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtBotTot", null, null, new LookupSettings(), EVariableType.Var, null), func4740());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtBot", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4737, o309, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o310 = new O.Assignment();
            foreach (IVariable listloop_a3681 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o310)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4742 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3681), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtTop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3681), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tTop", null, null, new LookupSettings(), EVariableType.Var, null), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3681), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3681), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3681), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPositiveNetCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3681))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3681), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "ShareOfIncAboveThreshold", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3681)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtTop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4742, o310, listloop_a3681)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4746 = () =>
            {
                var smplCommandRemember4747 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4745 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4745.SetZero(smpl);

                foreach (IVariable listloop_a4744 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4745.InjectAdd(smpl, temp4745, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4744), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtTop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4744), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4744), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4744)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4747;
                return temp4745;

            };


            O.Assignment o311 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4743 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtTopTot", null, null, new LookupSettings(), EVariableType.Var, null), func4746());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtTop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4743, o311, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o312 = new O.Assignment();
            foreach (IVariable listloop_a3682 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o312)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4748 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3682), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtMun", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3682), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tMun", null, null, new LookupSettings(), EVariableType.Var, null), O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3682), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTaxableInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3682), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3682), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3682))), O.Add(smpl, i4749, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3682), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "Adj_vtMun", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3682))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtMun", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4748, o312, listloop_a3682)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4753 = () =>
            {
                var smplCommandRemember4754 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4752 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4752.SetZero(smpl);

                foreach (IVariable listloop_a4751 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4752.InjectAdd(smpl, temp4752, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4751), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtMun", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4751), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4751), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4751)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4754;
                return temp4752;

            };


            O.Assignment o313 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4750 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtMunTot", null, null, new LookupSettings(), EVariableType.Var, null), func4753());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtMun", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4750, o313, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o314 = new O.Assignment();
            foreach (IVariable listloop_a3683 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o314)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4755 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3683), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtChu", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3683), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tChu", null, null, new LookupSettings(), EVariableType.Var, null), O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3683), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTaxableInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3683), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3683), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3683))), O.Add(smpl, i4756, O.Lookup(smpl, null, null, "Adj_vtChu", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtChu", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4755, o314, listloop_a3683)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4760 = () =>
            {
                var smplCommandRemember4761 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4759 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4759.SetZero(smpl);

                foreach (IVariable listloop_a4758 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4759.InjectAdd(smpl, temp4759, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4758), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtChu", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4758), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4758), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4758)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4761;
                return temp4759;

            };


            O.Assignment o315 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4757 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtChuTot", null, null, new LookupSettings(), EVariableType.Var, null), func4760());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtChu", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4757, o315, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o316 = new O.Assignment();
            foreach (IVariable listloop_a3684 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o316)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4762 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3684), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3684), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3684), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3684), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3684), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3684)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3684), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3684)), O.Lookup(smpl, null, null, "Adj_vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtSourceRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4762, o316, listloop_a3684)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4766 = () =>
            {
                var smplCommandRemember4767 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4765 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4765.SetZero(smpl);

                foreach (IVariable listloop_a4764 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4765.InjectAdd(smpl, temp4765, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4764), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4764), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4764), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4764)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4767;
                return temp4765;

            };


            O.Assignment o317 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4763 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtSourceRestTot", null, null, new LookupSettings(), EVariableType.Var, null), func4766());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtSourceRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4763, o317, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o318 = new O.Assignment();
            foreach (IVariable listloop_a3685 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o318)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4768 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3685), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtAM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3685), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tAM", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3685), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3685), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3685), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3685)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3685), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3685))), O.Add(smpl, i4769, O.Lookup(smpl, null, null, "Adj_vtAM", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4768, o318, listloop_a3685)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4773 = () =>
            {
                var smplCommandRemember4774 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4772 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4772.SetZero(smpl);

                foreach (IVariable listloop_a4771 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4772.InjectAdd(smpl, temp4772, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4771), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4771), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4771), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4771)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4774;
                return temp4772;

            };


            O.Assignment o319 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4770 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtAMTot", null, null, new LookupSettings(), EVariableType.Var, null), func4773());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4770, o319, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o320 = new O.Assignment();
            foreach (IVariable listloop_a3686 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o320)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4775 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3686), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3686), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "tCapPension", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3686), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionReceiveCap", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3686)), O.Add(smpl, i4776, O.Lookup(smpl, null, null, "Adj_vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtHHIncRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4775, o320, listloop_a3686)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4780 = () =>
            {
                var smplCommandRemember4781 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4779 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4779.SetZero(smpl);

                foreach (IVariable listloop_a4778 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4779.InjectAdd(smpl, temp4779, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4778), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4778), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4778), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4778)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4781;
                return temp4779;

            };


            O.Assignment o321 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4777 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtHHIncRestTot", null, null, new LookupSettings(), EVariableType.Var, null), func4780());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtHHIncRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4777, o321, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o322 = new O.Assignment();
            foreach (IVariable listloop_sp3687 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, o322)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4782 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3687), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3687), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3687), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3687), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp3687), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEBT", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp3687)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4782, o322, listloop_sp3687)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4786 = () =>
            {
                var smplCommandRemember4787 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4785 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4785.SetZero(smpl);

                foreach (IVariable listloop_sp4784 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sp")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4785.InjectAdd(smpl, temp4785, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_sp4784), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sp4784));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4787;
                return temp4785;

            };


            O.Assignment o323 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4783 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtCorp_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4786()), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Lookup(smpl, null, null, "adj_vtCorp", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4783, o323, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4791 = () =>
            {
                var smplCommandRemember4792 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4790 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4790.SetZero(smpl);

                foreach (IVariable listloop_a4789 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4790.InjectAdd(smpl, temp4790, O.Multiply(smpl, O.Lookup(smpl, null, null, "vtMediaExo", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4789), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4789)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4792;
                return temp4790;

            };


            O.Assignment o324 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4788 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vtMediaTot", null, null, new LookupSettings(), EVariableType.Var, null), func4791());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtMedia", null, ivTmpvar4788, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o324)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o325 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4793 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vtIndirect", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtVAT", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtDuty", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtSubDuty", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtProduction", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtCus", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vtEU", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vtIndirect", null, ivTmpvar4793, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o325)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o326 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4794 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovRevRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtBequest", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Lookup(smpl, null, null, "vGovDepr", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContribution", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Lookup(smpl, null, null, "vGovReceiveForeign", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovReceiveHHFirms", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovRent", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovProfit", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Adj_vGovRev", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovRevRest", null, ivTmpvar4794, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o326)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o327 = new O.Assignment();
            foreach (IVariable listloop_a3688 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o327)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4795 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vContribution", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContFreeRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContMandatory", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3688), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3688));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vContribution", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4795, o327, listloop_a3688)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o328 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4796 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vContributionTot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContUnemp", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContFreeRest", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContMandatory", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vContribution", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4796, o328, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o329 = new O.Assignment();
            foreach (IVariable listloop_a3689 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o329)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4797 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689), O.Multiply(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionPaymentMain", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionPaymentATP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionPaymentCap", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionReceiveMain", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPensionReceiveATP", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689)), O.Add(smpl, i4798, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3689), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "Adj_vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3689))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4797, o329, listloop_a3689)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4802 = () =>
            {
                var smplCommandRemember4803 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4801 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4801.SetZero(smpl);

                foreach (IVariable listloop_a4800 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4801.InjectAdd(smpl, temp4801, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4800), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4800), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4800), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4800)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4803;
                return temp4801;

            };


            O.Assignment o330 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4799 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vPersIncTot", null, null, new LookupSettings(), EVariableType.Var, null), func4802());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4799, o330, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o331 = new O.Assignment();
            foreach (IVariable listloop_a3690 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o331)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4804 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTaxableInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690), O.Multiply(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPersInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vNetCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEITC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vUnempAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEarlyRetAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3690), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vOtherAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3690)), O.Add(smpl, i4805, O.Lookup(smpl, null, null, "Adj_vTaxableInc", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTaxableInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4804, o331, listloop_a3690)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4809 = () =>
            {
                var smplCommandRemember4810 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4808 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4808.SetZero(smpl);

                foreach (IVariable listloop_a4807 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4808.InjectAdd(smpl, temp4808, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4807), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTaxableInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4807), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4807), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4807)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4810;
                return temp4808;

            };


            O.Assignment o332 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4806 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTaxableIncTot", null, null, new LookupSettings(), EVariableType.Var, null), func4809());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTaxableInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4806, o332, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o333 = new O.Assignment();
            foreach (IVariable listloop_a3691 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o333)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4811 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3691), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vNetCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3691), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3691), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPositiveCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3691)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3691), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vNegativeCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3691));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vNetCapInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4811, o333, listloop_a3691)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o334 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4812 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vNetCapIncTot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPositiveCapInc", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vNegativeCapInc", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vNetCapInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4812, o334, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o335 = new O.Assignment();
            foreach (IVariable listloop_a3692 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o335)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4813 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3692), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vPositiveNetCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3692), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3692), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPositiveCapInc", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3692), d4814));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPositiveNetCapInc", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4813, o335, listloop_a3692)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o336 = new O.Assignment();
            foreach (IVariable listloop_a3693 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o336)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4815 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3693), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vPersAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3693), O.Multiply(smpl, O.Lookup(smpl, null, null, "vPersAllowanceMax", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3693), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "ShareOfPopUsingPersAllow", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3693)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vPersAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4815, o336, listloop_a3693)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o337 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4816 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vPersAllowanceMax", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4817)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vPersAllowanceMax", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i4817)
            ), O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null))), O.Lookup(smpl, null, null, "adj_vPersAllowanceMax", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vPersAllowanceMax", null, ivTmpvar4816, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o337)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o338 = new O.Assignment();
            foreach (IVariable listloop_a3694 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o338)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4818 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3694), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vEITC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3694), O.Multiply(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3694), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "tavgEITC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3694), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3694), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3694), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3694), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3694)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3694), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3694))), O.Add(smpl, i4819, O.Lookup(smpl, null, null, "Adj_vEITC", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEITC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4818, o338, listloop_a3694)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4823 = () =>
            {
                var smplCommandRemember4824 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4822 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4822.SetZero(smpl);

                foreach (IVariable listloop_a4821 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4822.InjectAdd(smpl, temp4822, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4821), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEITC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4821), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4821), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4821)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4824;
                return temp4822;

            };


            O.Assignment o339 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4820 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vEITCTot", null, null, new LookupSettings(), EVariableType.Var, null), func4823());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEITC", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4820, o339, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o340 = new O.Assignment();
            foreach (IVariable listloop_a3695 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o340)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4825 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3695), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vUnempAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3695), O.Multiply(smpl, O.Lookup(smpl, null, null, "vUnempAllowanceExo", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3695), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3695)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vUnempAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4825, o340, listloop_a3695)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4829 = () =>
            {
                var smplCommandRemember4830 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4828 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4828.SetZero(smpl);

                foreach (IVariable listloop_a4827 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4828.InjectAdd(smpl, temp4828, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4827), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vUnempAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4827), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4827), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4827)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4830;
                return temp4828;

            };


            O.Assignment o341 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4826 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vUnempAllowanceTot", null, null, new LookupSettings(), EVariableType.Var, null), func4829());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vUnempAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4826, o341, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o342 = new O.Assignment();
            foreach (IVariable listloop_a3696 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o342)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4831 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3696), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vEarlyRetAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3696), O.Multiply(smpl, O.Lookup(smpl, null, null, "vEarlyRetAllowanceExo", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3696), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vContEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3696)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEarlyRetAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4831, o342, listloop_a3696)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4835 = () =>
            {
                var smplCommandRemember4836 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4834 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4834.SetZero(smpl);

                foreach (IVariable listloop_a4833 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4834.InjectAdd(smpl, temp4834, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4833), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vEarlyRetAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4833), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4833), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4833)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4836;
                return temp4834;

            };


            O.Assignment o343 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4832 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vEarlyRetAllowanceTot", null, null, new LookupSettings(), EVariableType.Var, null), func4835());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vEarlyRetAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4832, o343, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o344 = new O.Assignment();
            foreach (IVariable listloop_a3697 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o344)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4837 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3697), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vOtherAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3697), O.Multiply(smpl, O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "vOtherAllowanceExo", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3697), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nLaborForce", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3697)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3697), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3697)), O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vOtherAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4837, o344, listloop_a3697)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4841 = () =>
            {
                var smplCommandRemember4842 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4840 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4840.SetZero(smpl);

                foreach (IVariable listloop_a4839 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4840.InjectAdd(smpl, temp4840, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4839), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vOtherAllowance", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4839), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4839), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4839)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4842;
                return temp4840;

            };


            O.Assignment o345 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4838 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vOtherAllowanceTot", null, null, new LookupSettings(), EVariableType.Var, null), func4841());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vOtherAllowance", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4838, o345, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o346 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4843 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovExp", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTrans", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Lookup(smpl, null, null, "vGovInv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovSub", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovExpRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovExp", null, ivTmpvar4843, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o346)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o347 = new O.Assignment();
            foreach (IVariable listloop_a3698 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o347)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4844 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3698), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTrans", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3698), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3698), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3698)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3698), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3698)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3698), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3698));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTrans", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4844, o347, listloop_a3698)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4848 = () =>
            {
                var smplCommandRemember4849 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4847 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4847.SetZero(smpl);

                foreach (IVariable listloop_a4846 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4847.InjectAdd(smpl, temp4847, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4846), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTrans", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4846), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4846), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4846)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4849;
                return temp4847;

            };


            O.Assignment o348 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4845 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTrans_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4848());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTrans", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4845, o348, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o349 = new O.Assignment();
            foreach (IVariable listloop_a3699 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o349)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4850 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOutsideLF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3699), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3699)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransLabMarket", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4850, o349, listloop_a3699)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4854 = () =>
            {
                var smplCommandRemember4855 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4853 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4853.SetZero(smpl);

                foreach (IVariable listloop_a4852 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4853.InjectAdd(smpl, temp4853, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4852), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4852), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4852), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4852)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4855;
                return temp4853;

            };


            O.Assignment o350 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4851 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransLabMarket_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4854());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransLabMarket", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4851, o350, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o351 = new O.Assignment();
            foreach (IVariable listloop_a3700 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o351)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4856 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3700), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3700), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4857)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3700), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3700), O.Negate(smpl, i4857)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3700), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3700));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4856, o351, listloop_a3700)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4861 = () =>
            {
                var smplCommandRemember4862 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4860 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4860.SetZero(smpl);

                foreach (IVariable listloop_a4859 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4860.InjectAdd(smpl, temp4860, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4859), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4859), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4859), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nUnemployed", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4859)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4862;
                return temp4860;

            };


            O.Assignment o352 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4858 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransUnemp_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4861());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4858, o352, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o353 = new O.Assignment();
            foreach (IVariable listloop_a3701 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o353)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4863 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3701), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3701), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4864)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3701), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3701), O.Negate(smpl, i4864)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3701), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3701));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4863, o353, listloop_a3701)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4868 = () =>
            {
                var smplCommandRemember4869 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4867 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4867.SetZero(smpl);

                foreach (IVariable listloop_a4866 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4867.InjectAdd(smpl, temp4867, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4866), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4866), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4866), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOutsideLF", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4866)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4869;
                return temp4867;

            };


            O.Assignment o354 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4865 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransLabMarketRest_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4868());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4865, o354, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o355 = new O.Assignment();
            foreach (IVariable listloop_a3702 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o355)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4870 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3702), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3702)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransPensions", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4870, o355, listloop_a3702)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4874 = () =>
            {
                var smplCommandRemember4875 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4873 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4873.SetZero(smpl);

                foreach (IVariable listloop_a4872 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4873.InjectAdd(smpl, temp4873, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4872), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4872), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4872), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4872)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4875;
                return temp4873;

            };


            O.Assignment o356 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4871 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransPensions_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4874());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransPensions", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4871, o356, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o357 = new O.Assignment();
            foreach (IVariable listloop_a3703 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o357)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4876 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3703), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3703), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4877)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3703), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3703), O.Negate(smpl, i4877)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3703), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3703));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4876, o357, listloop_a3703)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4881 = () =>
            {
                var smplCommandRemember4882 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4880 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4880.SetZero(smpl);

                foreach (IVariable listloop_a4879 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4880.InjectAdd(smpl, temp4880, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4879), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4879), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4879), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4879)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4882;
                return temp4880;

            };


            O.Assignment o358 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4878 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransOldAgePens_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4881());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4878, o358, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o359 = new O.Assignment();
            foreach (IVariable listloop_a3704 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o359)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4883 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3704), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3704), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4884)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3704), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3704), O.Negate(smpl, i4884)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3704), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3704));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4883, o359, listloop_a3704)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4888 = () =>
            {
                var smplCommandRemember4889 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4887 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4887.SetZero(smpl);

                foreach (IVariable listloop_a4886 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4887.InjectAdd(smpl, temp4887, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4886), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4886), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4886), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4886)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4889;
                return temp4887;

            };


            O.Assignment o360 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4885 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransWornPens_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4888());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4885, o360, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o361 = new O.Assignment();
            foreach (IVariable listloop_a3705 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o361)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4890 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3705), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3705), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4891)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3705), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3705), O.Negate(smpl, i4891)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3705), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3705));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4890, o361, listloop_a3705)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4895 = () =>
            {
                var smplCommandRemember4896 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4894 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4894.SetZero(smpl);

                foreach (IVariable listloop_a4893 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4894.InjectAdd(smpl, temp4894, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4893), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4893), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4893), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4893)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4896;
                return temp4894;

            };


            O.Assignment o362 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4892 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransEarlyRet_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4895());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4892, o362, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o363 = new O.Assignment();
            foreach (IVariable listloop_a3706 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o363)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4897 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3706), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3706), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4898)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3706), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3706), O.Negate(smpl, i4898)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3706), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3706));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4897, o363, listloop_a3706)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4902 = () =>
            {
                var smplCommandRemember4903 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4901 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4901.SetZero(smpl);

                foreach (IVariable listloop_a4900 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4901.InjectAdd(smpl, temp4901, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4900), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4900), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4900), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4900)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4903;
                return temp4901;

            };


            O.Assignment o364 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4899 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransCivilServants_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4902());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4899, o364, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o365 = new O.Assignment();
            foreach (IVariable listloop_a3707 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o365)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4904 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3707), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3707), O.Multiply(smpl, O.Lookup(smpl, null, null, "fGov", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4905)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3707), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3707), O.Negate(smpl, i4905)
                ))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3707), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3707));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4904, o365, listloop_a3707)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4909 = () =>
            {
                var smplCommandRemember4910 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4908 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4908.SetZero(smpl);

                foreach (IVariable listloop_a4907 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4908.InjectAdd(smpl, temp4908, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4907), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4907), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4907), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4907), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4907), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4907))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4910;
                return temp4908;

            };


            O.Assignment o366 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4906 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransPensOther_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4909());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4906, o366, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o367 = new O.Assignment();
            foreach (IVariable listloop_a3708 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o367)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4911 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransFamily", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708)), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708), O.Subtract(smpl, O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, (new ScalarString("l"))), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), (new ScalarString("l"))), listloop_a3708))), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, (new ScalarString("l"))), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), (new ScalarString("l"))), listloop_a3708))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3708), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3708)), O.Divide(smpl, O.Lookup(smpl, null, null, "Adj_vTrans", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4911, o367, listloop_a3708)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4915 = () =>
            {
                var smplCommandRemember4916 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4914 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4914.SetZero(smpl);

                foreach (IVariable listloop_a4913 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4914.InjectAdd(smpl, temp4914, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4913), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4913), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4913), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4913)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4916;
                return temp4914;

            };


            O.Assignment o368 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4912 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransOther_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4915());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4912, o368, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o369 = new O.Assignment();
            foreach (IVariable listloop_a3709 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o369)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4917 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3709), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3709), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4918)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3709), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3709), O.Negate(smpl, i4918)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3709), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3709));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4917, o369, listloop_a3709)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4922 = () =>
            {
                var smplCommandRemember4923 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4921 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4921.SetZero(smpl);

                foreach (IVariable listloop_a4920 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4921.InjectAdd(smpl, temp4921, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4920), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4920), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4920), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4920)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4923;
                return temp4921;

            };


            O.Assignment o370 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4919 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransGreen_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4922());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4919, o370, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o371 = new O.Assignment();
            foreach (IVariable listloop_a3710 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o371)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4924 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3710), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3710), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4925)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3710), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3710), O.Negate(smpl, i4925)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3710), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3710));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4924, o371, listloop_a3710)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4929 = () =>
            {
                var smplCommandRemember4930 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4928 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4928.SetZero(smpl);

                foreach (IVariable listloop_a4927 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4928.InjectAdd(smpl, temp4928, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4927), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4927), O.Subtract(smpl, O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4927), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4927), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4927), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4927)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4927), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4927))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4930;
                return temp4928;

            };


            O.Assignment o372 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4926 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransHouseOther_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4929());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4926, o372, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o373 = new O.Assignment();
            foreach (IVariable listloop_a3711 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o373)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4931 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3711), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3711), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4932)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3711), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3711), O.Negate(smpl, i4932)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3711), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3711));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4931, o373, listloop_a3711)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4936 = () =>
            {
                var smplCommandRemember4937 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4935 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4935.SetZero(smpl);

                foreach (IVariable listloop_a4934 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4935.InjectAdd(smpl, temp4935, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4934), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4934), O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4934), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4934), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4934), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nWornPens", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4934))));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4937;
                return temp4935;

            };


            O.Assignment o374 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4933 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransHousePens_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4936());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4933, o374, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o375 = new O.Assignment();
            foreach (IVariable listloop_a3712 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o375)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4938 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3712), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3712), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4939)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3712), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3712), O.Negate(smpl, i4939)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3712), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3712));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4938, o375, listloop_a3712)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4943 = () =>
            {
                var smplCommandRemember4944 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4942 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4942.SetZero(smpl);

                foreach (IVariable listloop_a4941 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4942.InjectAdd(smpl, temp4942, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4941), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4941), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4941), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4941)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4944;
                return temp4942;

            };


            O.Assignment o376 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4940 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransRestTaxable_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4943());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4940, o376, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o377 = new O.Assignment();
            foreach (IVariable listloop_a3713 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o377)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar4945 = O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3713), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3713), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i4946)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3713), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3713), O.Negate(smpl, i4946)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a3713), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "adj_vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a3713));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4945, o377, listloop_a3713)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func4950 = () =>
            {
                var smplCommandRemember4951 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp4949 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4949.SetZero(smpl);

                foreach (IVariable listloop_a4948 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp4949.InjectAdd(smpl, temp4949, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4948), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4948), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4948), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a4948)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember4951;
                return temp4949;

            };


            O.Assignment o378 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4947 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vTransRestNontaxable_tot", null, null, new LookupSettings(), EVariableType.Var, null), func4950());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar4947, o378, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o379 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4952 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovExpRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovExpRest", null, ivTmpvar4952, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o379)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o380 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4953 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "GDPShareGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovLandRights", null, ivTmpvar4953, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o380)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o381 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4954 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "GDPShareGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovPaymentForeign", null, ivTmpvar4954, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o381)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o382 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4955 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "GDPShareGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovPaymentHHFirms", null, ivTmpvar4955, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o382)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o383 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4956 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "GDPShareGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovPaymentHH", null, ivTmpvar4956, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o383)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o384 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4957 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "GDPShareGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGovPaymentFirms", null, ivTmpvar4957, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o384)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o385 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4958 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovPrimBalance", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vGovRev", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovExp", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovPrimBalance", null, ivTmpvar4958, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o385)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o386 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4959 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovRev", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vtDirect", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtIndirect", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovRevRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovRev", null, ivTmpvar4959, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o386)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o387 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4960 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtDirect", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vtSource", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtAM", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtHHWeight", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtCorp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtPAL", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtMedia", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtDirect", null, ivTmpvar4960, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o387)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o388 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4961 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtSource", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vtBot", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtTop", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtMun", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtChu", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtProp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtStock", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtSource", null, ivTmpvar4961, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o388)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o389 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4962 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtBot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtBot", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtBot", null, ivTmpvar4962, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o389)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o390 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4963 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtTop", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtTop", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtTop", null, ivTmpvar4963, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o390)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o391 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4964 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtMun", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtMun", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtMun", null, ivTmpvar4964, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o391)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o392 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4965 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtChu", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtChu", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtChu", null, ivTmpvar4965, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o392)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o393 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4966 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtProp", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtProp", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtProp", null, ivTmpvar4966, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o393)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o394 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4967 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtStock", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtStock", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtStock", null, ivTmpvar4967, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o394)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o395 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4968 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtSourceRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtSourceRest", null, ivTmpvar4968, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o395)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o396 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4969 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtAM", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtAM", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtAM", null, ivTmpvar4969, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o396)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o397 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4970 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtHHIncRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtHHIncRest", null, ivTmpvar4970, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o397)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o398 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4971 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtHHWeight", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtHHWeight", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtHHWeight", null, ivTmpvar4971, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o398)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o399 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4972 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#sTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vtCorp", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#sTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtCorp", null, ivTmpvar4972, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o399)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o400 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4973 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtPal", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtPal", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtPal", null, ivTmpvar4973, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o400)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o401 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4974 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtMedia", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtMedia", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtMedia", null, ivTmpvar4974, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o401)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o402 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4975 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtIndirect", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vtVAT", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtDuty", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtSubDuty", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtProduction", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtCus", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vtEU", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtIndirect", null, ivTmpvar4975, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o402)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o403 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4976 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtVAT", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtVAT", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtVAT", null, ivTmpvar4976, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o403)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o404 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4977 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtDuty", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtDuty", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtDuty", null, ivTmpvar4977, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o404)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o405 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4978 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtSubDuty", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtSubDuty", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtSubDuty", null, ivTmpvar4978, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o405)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o406 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4979 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtProduction", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtProduction", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtProduction", null, ivTmpvar4979, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o406)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o407 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4980 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtCus", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtCus", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtCus", null, ivTmpvar4980, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o407)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o408 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4981 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vtEU", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vtEU", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vtEU", null, ivTmpvar4981, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o408)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o409 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4982 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovRevRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovRevRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovRevRest", null, ivTmpvar4982, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o409)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o410 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4983 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovExp", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vG", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTrans", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovInv", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovSub", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovExpRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovExp", null, ivTmpvar4983, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o410)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o411 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4984 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vG", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#gtot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#gtot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vG", null, ivTmpvar4984, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o411)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o412 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4985 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTrans", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransOther", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTrans", null, ivTmpvar4985, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o412)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o413 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4986 = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransLabMarket", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransLabMarket", null, ivTmpvar4986, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o413)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o414 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4987 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransUnemp", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransUnemp", null, ivTmpvar4987, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o414)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o415 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4988 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransLabMarketRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransLabMarketRest", null, ivTmpvar4988, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o415)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o416 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4989 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransPensions", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransPensions", null, ivTmpvar4989, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o416)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o417 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4990 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransOldAgePens", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransOldAgePens", null, ivTmpvar4990, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o417)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o418 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4991 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransWornPens", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransWornPens", null, ivTmpvar4991, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o418)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o419 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4992 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransEarlyRet", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransEarlyRet", null, ivTmpvar4992, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o419)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o420 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4993 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransCivilServants", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransCivilServants", null, ivTmpvar4993, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o420)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o421 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4994 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransPensOther", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransPensOther", null, ivTmpvar4994, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o421)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o422 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4995 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransOther", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vTransFamily", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Adj_vTrans", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransOther", null, ivTmpvar4995, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o422)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o423 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4996 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransFamily", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransFamily", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransFamily", null, ivTmpvar4996, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o423)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o424 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4997 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransGreen", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransGreen", null, ivTmpvar4997, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o424)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o425 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4998 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHouseOther", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransHouseOther", null, ivTmpvar4998, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o425)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o426 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4999 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransHousePens", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransHousePens", null, ivTmpvar4999, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o426)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o427 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5000 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestTaxable", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransRestTaxable", null, ivTmpvar5000, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o427)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o428 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5001 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vTransRestNontaxable", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#aTot", null, null, new LookupSettings(), EVariableType.Var, null)
            ));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vTransRestNontaxable", null, ivTmpvar5001, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o428)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o429 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5002 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovInv", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovInv", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovInv", null, ivTmpvar5002, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o429)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o430 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5003 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovSub", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovSub", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovSub", null, ivTmpvar5003, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o430)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o431 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5004 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovExpRest", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Struc_vGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "Struc_vGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovExpRest", null, ivTmpvar5004, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o431)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o432 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5005 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovLandRights", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovLandRights", null, ivTmpvar5005, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o432)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o433 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5006 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovPaymentForeign", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovPaymentForeign", null, ivTmpvar5006, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o433)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o434 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5007 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovPaymentHHFirms", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovPaymentHHFirms", null, ivTmpvar5007, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o434)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o435 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5008 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovPaymentFirms", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovPaymentFirms", null, ivTmpvar5008, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o435)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o436 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5009 = O.Add(smpl, O.Lookup(smpl, null, null, "J_Struc_vGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "vGovPaymentHH", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "Struc_vGovPaymentHH", null, ivTmpvar5009, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o436)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func5014 = () =>
            {
                var smplCommandRemember5015 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5013 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5013.SetZero(smpl);

                foreach (IVariable listloop_s5011 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5013.InjectAdd(smpl, temp5013, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5012)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5011), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5011), O.Negate(smpl, i5012)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5011), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5011)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5015;
                return temp5013;

            };


            O.Assignment o437 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5010 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qY_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func5014(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5016)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5016)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5010, o437, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o438 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5017 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pY_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5017, o438, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func5022 = () =>
            {
                var smplCommandRemember5023 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5021 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5021.SetZero(smpl);

                foreach (IVariable listloop_s5019 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5021.InjectAdd(smpl, temp5021, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5020)
                    ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5019), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5019), O.Negate(smpl, i5020)
                    ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5019), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5019)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5023;
                return temp5021;

            };


            O.Assignment o439 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5018 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qM_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func5022(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5024)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5024)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5018, o439, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o440 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5025 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pM_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5025, o440, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o441 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5026 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5027)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5027)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5028)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pG", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5028)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5029)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot")), O.Negate(smpl, i5029)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot")))), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5030)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5030)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5031)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5031)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5032)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "pGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i5032)
            )));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "qGDP", null, ivTmpvar5026, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o441)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o442 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5033 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Lookup(smpl, null, null, "vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "qGDP", null, null, new LookupSettings(), EVariableType.Var, null)));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "pGDP", null, ivTmpvar5033, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o442)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o443 = new O.Assignment();
            foreach (IVariable listloop_s3714 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o443)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5034 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714), O.Divide(smpl, O.Subtract(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5035)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714), O.Negate(smpl, i5035)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714)), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5036)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714), O.Negate(smpl, i5036)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5037)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3714), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3714), O.Negate(smpl, i5037)
                )));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5034, o443, listloop_s3714)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o444 = new O.Assignment();
            foreach (IVariable listloop_s3715 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o444)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5038 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3715), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_pGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3715), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3715), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3715), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3715), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3715)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5038, o444, listloop_s3715)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o445 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5039 = O.Add(smpl, O.Lookup(smpl, null, null, "J_qGrossValAdd_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Subtract(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5040)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5040)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5041)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5041)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5042)
            ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Negate(smpl, i5042)
            )));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5039, o445, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o446 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5043 = O.Add(smpl, O.Lookup(smpl, null, null, "J_pGrossValAdd_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5043, o446, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func5047 = (IVariable listloop_s3716) =>
            {
                var smplCommandRemember5048 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5046 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5046.SetZero(smpl);

                foreach (IVariable listloop_c5045 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5046.InjectAdd(smpl, temp5046, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c5045, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c5045, listloop_s3716));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5048;
                return temp5046;

            };

            Func<IVariable, IVariable> func5051 = (IVariable listloop_s3716) =>
            {
                var smplCommandRemember5052 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5050 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5050.SetZero(smpl);

                foreach (IVariable listloop_i5049 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5050.InjectAdd(smpl, temp5050, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i5049, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i5049, listloop_s3716));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5052;
                return temp5050;

            };

            Func<IVariable, IVariable> func5055 = (IVariable listloop_s3716) =>
            {
                var smplCommandRemember5056 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5054 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5054.SetZero(smpl);

                foreach (IVariable listloop_r5053 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5054.InjectAdd(smpl, temp5054, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r5053, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r5053, listloop_s3716));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5056;
                return temp5054;

            };

            Func<IVariable, IVariable> func5059 = (IVariable listloop_s3716) =>
            {
                var smplCommandRemember5060 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5058 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5058.SetZero(smpl);

                foreach (IVariable listloop_g5057 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5058.InjectAdd(smpl, temp5058, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g5057, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g5057, listloop_s3716));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5060;
                return temp5058;

            };

            Func<IVariable, IVariable> func5063 = (IVariable listloop_s3716) =>
            {
                var smplCommandRemember5064 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5062 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5062.SetZero(smpl);

                foreach (IVariable listloop_x5061 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5062.InjectAdd(smpl, temp5062, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x5061, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x5061, listloop_s3716));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5064;
                return temp5062;

            };


            O.Assignment o447 = new O.Assignment();
            foreach (IVariable listloop_s3716 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o447)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5044 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3716), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3716), func5047(listloop_s3716)), func5051(listloop_s3716)), func5055(listloop_s3716)), func5059(listloop_s3716)), func5063(listloop_s3716));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5044, o447, listloop_s3716)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o448 = new O.Assignment();
            foreach (IVariable listloop_s3717 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o448)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5065 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3717), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3717), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3717), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3717), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3717), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3717)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5065, o448, listloop_s3717)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func5069 = () =>
            {
                var smplCommandRemember5070 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5068 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5068.SetZero(smpl);

                foreach (IVariable listloop_s5067 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5068.InjectAdd(smpl, temp5068, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5067), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5067));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5070;
                return temp5068;

            };


            O.Assignment o449 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5066 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vY_tot", null, null, new LookupSettings(), EVariableType.Var, null), func5069());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5066, o449, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable> func5074 = (IVariable listloop_s3718) =>
            {
                var smplCommandRemember5075 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5073 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5073.SetZero(smpl);

                foreach (IVariable listloop_c5072 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5073.InjectAdd(smpl, temp5073, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_c5072, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qC_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_c5072, listloop_s3718));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5075;
                return temp5073;

            };

            Func<IVariable, IVariable> func5078 = (IVariable listloop_s3718) =>
            {
                var smplCommandRemember5079 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5077 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5077.SetZero(smpl);

                foreach (IVariable listloop_i5076 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5077.InjectAdd(smpl, temp5077, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i5076, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qI_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i5076, listloop_s3718));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5079;
                return temp5077;

            };

            Func<IVariable, IVariable> func5082 = (IVariable listloop_s3718) =>
            {
                var smplCommandRemember5083 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5081 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5081.SetZero(smpl);

                foreach (IVariable listloop_r5080 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("r")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5081.InjectAdd(smpl, temp5081, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_r5080, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qR_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_r5080, listloop_s3718));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5083;
                return temp5081;

            };

            Func<IVariable, IVariable> func5086 = (IVariable listloop_s3718) =>
            {
                var smplCommandRemember5087 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5085 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5085.SetZero(smpl);

                foreach (IVariable listloop_g5084 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("g")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5085.InjectAdd(smpl, temp5085, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_g5084, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qG_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_g5084, listloop_s3718));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5087;
                return temp5085;

            };

            Func<IVariable, IVariable> func5090 = (IVariable listloop_s3718) =>
            {
                var smplCommandRemember5091 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5089 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5089.SetZero(smpl);

                foreach (IVariable listloop_x5088 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5089.InjectAdd(smpl, temp5089, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_x5088, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qX_m", null, null, new LookupSettings(), EVariableType.Var, null), listloop_x5088, listloop_s3718));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5091;
                return temp5089;

            };


            O.Assignment o450 = new O.Assignment();
            foreach (IVariable listloop_s3718 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o450)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5071 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3718), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_qM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3718), func5074(listloop_s3718)), func5078(listloop_s3718)), func5082(listloop_s3718)), func5086(listloop_s3718)), func5090(listloop_s3718));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5071, o450, listloop_s3718)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o451 = new O.Assignment();
            foreach (IVariable listloop_s3719 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o451)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5092 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3719), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3719), O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3719), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3719), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3719), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3719)));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5092, o451, listloop_s3719)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func5096 = () =>
            {
                var smplCommandRemember5097 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5095 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5095.SetZero(smpl);

                foreach (IVariable listloop_s5094 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5095.InjectAdd(smpl, temp5095, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5094), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5094));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5097;
                return temp5095;

            };


            O.Assignment o452 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5093 = O.Add(smpl, O.Lookup(smpl, null, null, "J_vM_tot", null, null, new LookupSettings(), EVariableType.Var, null), func5096());
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5093, o452, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o453 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5098 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGDP", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vC", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot"), new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vI", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vG", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vGDP", null, ivTmpvar5098, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o453)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o454 = new O.Assignment();
            foreach (IVariable listloop_s3720 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, o454)))
            {
                O.AdjustT0(smpl, -1);
                IVariable ivTmpvar5099 = O.Subtract(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3720), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "J_vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3720), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3720), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3720)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s3720), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s3720));
                O.AdjustT0(smpl, 1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5099, o454, listloop_s3720)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o455 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5100 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vGrossValAdd_tot", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vR", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5100, o455, new ScalarString("tot"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o456 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5101 = O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vBoP", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vX", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vM", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrHH", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Lookup(smpl, null, null, "vrGov", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vrEquity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vrFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Subtract(smpl, O.Lookup(smpl, null, null, "vNetForeignAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5102)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "vNetForeignAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i5102)
            ), O.Lookup(smpl, null, null, "fv", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vPrimIncomeRes", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vDispRes", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Subtract(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "w", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qL", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vW", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vBoP", null, ivTmpvar5101, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o456)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o457 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5103 = O.Subtract(smpl, O.Subtract(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_vNetForeignAssets", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("Tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vWealth", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("Tot"))), O.Lookup(smpl, null, null, "vGovWealth", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "vEquity", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vFirmDebt", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "vNetForeignAssets", null, ivTmpvar5103, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o457)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o458 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5104 = O.Add(smpl, O.Lookup(smpl, null, null, "J_LaborProductivity", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nEmployed", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "LaborProductivity", null, ivTmpvar5104, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o458)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o459 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5105 = O.Subtract(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "J_gap_qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_qGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot")))), i5106);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "gap_qGrossValAdd", null, ivTmpvar5105, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o459)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func5111 = () =>
            {
                var smplCommandRemember5112 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5110 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5110.SetZero(smpl);

                foreach (IVariable listloop_s5108 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5110.InjectAdd(smpl, temp5110, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5108), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5108), O.Subtract(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5108), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "prod_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5108), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_s5108), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "str_prod_s", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s5108)), i5109)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember5112;
                return temp5110;

            };


            O.Assignment o460 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5107 = O.Add(smpl, O.Lookup(smpl, null, null, "J_gap_productivity", null, null, new LookupSettings(), EVariableType.Var, null), O.Divide(smpl, func5111(), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("tot")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "vGrossValAdd", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("tot"))));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "gap_productivity", null, ivTmpvar5107, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o460)
            ;


            //[[splitSTOP]]


        }
    }
}
